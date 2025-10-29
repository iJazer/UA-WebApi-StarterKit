  /* eslint-disable @typescript-eslint/no-explicit-any */
import * as OpcUa from 'opcua-webapi';
import * as pako from 'pako';
import { Account } from './user/Account';
import { ICompletedRequest } from './service/ICompletedRequest';
import { IResponseBody } from './service/IResponseBody';

function toFault(request: ICompletedRequest, response?: IResponseBody): ICompletedRequest | null {
   if (response?.ResponseHeader) {
      const responseHeader = response.ResponseHeader;
      if (!responseHeader.ServiceResult || responseHeader.ServiceResult?.Code === 0) {
         return null;
      }
      const stringTable = response.ResponseHeader?.StringTable;
      const serviceDiagnostics = response.ResponseHeader?.ServiceDiagnostics;
      if (!stringTable?.length || !serviceDiagnostics) {
         return { ...request, code: responseHeader.ServiceResult?.Code };
      }
      let message = '';
      if ((serviceDiagnostics?.SymbolicId && serviceDiagnostics?.SymbolicId > 0) || serviceDiagnostics?.SymbolicId === 0) {
         message += `[${stringTable?.[serviceDiagnostics?.SymbolicId]}] `;
      }
      if ((serviceDiagnostics?.LocalizedText && serviceDiagnostics?.LocalizedText > 0) || serviceDiagnostics?.LocalizedText === 0) {
         message += `'${stringTable?.[serviceDiagnostics?.LocalizedText]}'`;
      }
      return {
         ...request,
         code: responseHeader.ServiceResult?.Code,
         error: message
      }
   }
   return {
      ...request,
      code: OpcUa.StatusCodes.BadUnknownResponse
   }
}

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

async function readResponseBody(url: string, response: Response) {
   const content = response.headers.get("Content-Type");
   if (content && content.indexOf("json") < 0) {
      // console.error("UnexpectedResponse: " + await response.text());
      return null;
   }
   return await response.clone().json();

   //const data = await response.arrayBuffer();
   //const view = new DataView(data);
   //console.log("URL: " + url + "[Size=" + view.byteLength + "]");

   //if (view.byteLength < 2) {
   //   console.error("EmptyBody!");
   //   return null;
   //}

   //let json = undefined;
   //if (view.getUint8(0) === 0x1f && view.getUint8(1) === 0x8b) {
   //   const decompressed = pako.deflate(new Uint8Array(data));
   //   json = new TextDecoder().decode(decompressed);
   //}
   //else {
   //   json = new TextDecoder().decode(data);
   //}

   //return JSON.parse(json);
}

export async function call(
   url: string,
   message: ICompletedRequest,
   controller?: AbortController,
   user?: Account,
   compress?: boolean) {
   const request = message.request.Body;
   const timeoutId = (request?.RequestHeader?.TimeoutHint && controller)
      ? setTimeout(() => controller.abort(), request?.RequestHeader?.TimeoutHint)
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
         const body = await readResponseBody(url, response);
         const fault = toFault(message, body);
         if (fault) {
            return fault;
         }
         return body;
      }
      else {
         console.info(`call: ${message.callerHandle} ${response.status} ${response.statusText}`);
         return {
            callerHandle: message.callerHandle,
            code: OpcUa.StatusCodes.BadUnexpectedError,
            message: `HTTP ${response.status} ${response.statusText}`
         };
      }
   }
   catch (exception: any) {
      if (timeoutId) clearTimeout(timeoutId);
      if (exception.code) {
         console.info(`call: ${message.callerHandle} ${exception.code} ${exception.message}`);
         return {
            callerHandle: message.callerHandle,
            ...exception
         };
      } else {
         console.info(`call: ${message.callerHandle} BadUnexpectedError ${exception.toString()}`);
         return {
            callerHandle: message.callerHandle,
            code: OpcUa.StatusCodes.BadUnexpectedError,
            message: exception.toString()
         };
      }
   }
}
