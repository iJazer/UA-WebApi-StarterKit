import * as React from 'react';
import pako from 'pako';
import * as OpcUa from './opcua';

import { Account } from './user/Account';
import { ICompletedRequest } from './service/ICompletedRequest';
import { IRequestMessage } from './service/IRequestMessage';
import { HandleFactory } from './opcua-utils';
import { IResponseMessage } from './service/IResponseMessage';
import { UserContext } from './UserProvider';

function stringToUtf8ByteArray(str: string): Uint8Array {
   const encoder = new TextEncoder();
   return encoder.encode(str);
}

async function gzip(data: string): Promise<Uint8Array> {
   return new Promise((resolve) => {
      const encoder = new TextEncoder();
      const compressedData = pako.gzip(encoder.encode(data));
      resolve(compressedData);
   });
}

// eslint-disable-next-line @typescript-eslint/no-explicit-any
async function readResponseBody(url: string, response: any) {
   console.log("URL: " + url);
   const content = response.headers.get("Content-Type");
   if (content && content.indexOf("json") < 0) {
      console.error("UnexpectedResponse: " + await response.text());
      return null;
   }
   // the fetch library automatically uncompresses gzipped content.
   return await response.json();
}

function toFault(result: ICompletedRequest): ICompletedRequest | null {
   const response = result?.response;
   if (response?.Body?.ResponseHeader) {
      const responseHeader = response.Body.ResponseHeader;
      if (!responseHeader.ServiceResult || responseHeader.ServiceResult === 0) {
         return null;
      }
      const stringTable = response.Body?.ResponseHeader?.StringTable;
      const serviceDiagnostics = response.Body?.ResponseHeader?.ServiceDiagnostics;
      if (!stringTable?.length || !serviceDiagnostics) {
         return { ...result, code: responseHeader.ServiceResult };
      }
      let message = '';
      if ((serviceDiagnostics?.SymbolicId && serviceDiagnostics?.SymbolicId > 0) || serviceDiagnostics?.SymbolicId === 0) {
         message += `[${stringTable?.[serviceDiagnostics?.SymbolicId]}] `;
      }
      if ((serviceDiagnostics?.LocalizedText && serviceDiagnostics?.LocalizedText > 0) || serviceDiagnostics?.LocalizedText === 0) {
         message += `'${stringTable?.[serviceDiagnostics?.LocalizedText]}'`;
      }
      return { ...result, code: responseHeader.ServiceResult, message: message }
   }
   return { ...result, code: OpcUa.StatusCodes.BadUnknownResponse }
}

async function call(
   url: string,
   result: ICompletedRequest,
   user?: Account,
   compress?: boolean) : Promise<ICompletedRequest> {

   const controller: AbortController = new AbortController();
   const request = result.request;
   const timeoutId = (request?.Body?.RequestHeader?.TimeoutHint && controller)
      ? setTimeout(() => controller.abort(), request?.Body?.RequestHeader?.TimeoutHint)
      : null;
   try {
      const json = JSON.stringify(request);
      const body = (compress) ? await gzip(json) : stringToUtf8ByteArray(json);
      const requestOptions = {
         method: 'POST',
         mode: 'cors',
         cache: 'no-cache',
         headers: {
            ...(compress ? { 'Content-Encoding': 'gzip' } : {}),
            'Content-Type': 'application/json',
            'Authorization': (user?.accessToken) ? `Bearer ${user?.accessToken}` : ''
         },
         credentials: 'include',
         body: body,
         signal: controller?.signal
      } as RequestInit;
      const response = await fetch(url, requestOptions);
      if (timeoutId) clearTimeout(timeoutId);
      if (response.ok) {
         result.response = await readResponseBody(url, response);
         const fault = toFault(result);
         if (fault) {
            return fault;
         }
         return result;
      }
      else {
         console.info(`call: ${response.status} ${response.statusText}`);
         return {
            ...result,
            code: OpcUa.StatusCodes.BadUnexpectedError,
            message: `HTTP ${response.status} ${response.statusText}`
         };
      }
   }
   // eslint-disable-next-line @typescript-eslint/no-explicit-any
   catch (exception: any) {
      if (timeoutId) clearTimeout(timeoutId);
      if (exception.code) {
         console.info(`call: ${exception.code} ${exception.message}`);
         return exception;
      } else {
         console.info(`call: BadUnexpectedError ${exception.toString()}`);
         return {
            ...result,
            code: OpcUa.StatusCodes.BadUnexpectedError,
            message: exception.toString()
         };
      }
   }
}

const serviceIdToUrl: { [key: number]: string } = {
   [OpcUa.ReadRequestMessageServiceIdEnum.NUMBER_629]: "read",
   [OpcUa.WriteRequestMessageServiceIdEnum.NUMBER_671]: "write",
   [OpcUa.CallRequestMessageServiceIdEnum.NUMBER_710]: "call",
   [OpcUa.BrowseRequestMessageServiceIdEnum.NUMBER_525]: "browse",
   [OpcUa.BrowseNextRequestMessageServiceIdEnum.NUMBER_531]: "browsenext",
   [OpcUa.TranslateBrowsePathsToNodeIdsRequestMessageServiceIdEnum.NUMBER_552]: "translate",
   [OpcUa.FindServersRequestMessageServiceIdEnum.NUMBER_420]: "findservers",
   [OpcUa.GetEndpointsRequestMessageServiceIdEnum.NUMBER_426]: "getendpoints",
   [OpcUa.HistoryReadRequestMessageServiceIdEnum.NUMBER_662]: "historyread",
   [OpcUa.HistoryUpdateResponseMessageServiceIdEnum.NUMBER_701]: "historyupdate",
};

interface UseSessionlessCallInternals {
   serverUrl: string | null,
   requestTimeout: number,
   requests: Map<number, ICompletedRequest>,
   responses: IResponseMessage[],
   lastCompletedRequest?: ICompletedRequest
}

function useSessionlessCall(initialServerUrl : string = "") {
   const [serverUrl, setServerUrl] = React.useState<string>(initialServerUrl);
   const [requestTimeout, setRequestTimeout] = React.useState<number>(60000);
   const [lastCompletedRequest, setLastCompletedRequest] = React.useState<ICompletedRequest | undefined>();
   const { user } = React.useContext(UserContext);

   const m = React.useRef<UseSessionlessCallInternals>({
      serverUrl: null,
      requestTimeout: 60000,
      requests: new Map<number, ICompletedRequest>(),
      responses: []
   });

   const processResponse = React.useCallback((result: ICompletedRequest) => {
         const requestHandle = result?.response?.Body?.ResponseHeader?.RequestHandle ?? 0;
         const request = m.current.requests.get(requestHandle);
         if (request) {
            m.current.requests.delete(requestHandle);
            m.current.lastCompletedRequest = { ...result };
            setLastCompletedRequest(m.current.lastCompletedRequest);
         }
   }, [])

   const sendRequest = React.useCallback((request: IRequestMessage, clientHandle?: number) => {
      if (!request.Body.RequestHeader) {
         request.Body.RequestHeader = {}
      }
      const requestHeader = request.Body.RequestHeader;
      requestHeader.RequestHandle = HandleFactory.increment();
      if (!requestHeader.Timestamp) {
         requestHeader.Timestamp = new Date();
      }
      if (!requestHeader.TimeoutHint) {
         requestHeader.TimeoutHint = requestTimeout;
      }
      const result: ICompletedRequest = { clientHandle: clientHandle ?? requestHeader.RequestHandle, request };
      m.current.requests.set(requestHeader.RequestHandle, result);
      call(
         `${serverUrl}/opcua/${serviceIdToUrl[request.ServiceId ?? 0]}`,
         result,
         user).then((result) => {
            processResponse(result);
         });
   }, [serverUrl, requestTimeout, user, processResponse]);

   return {
      serverUrl,
      setServerUrl,
      requestTimeout,
      setRequestTimeout,
      sendRequest,
      lastCompletedRequest: lastCompletedRequest
   };
}

export default useSessionlessCall;