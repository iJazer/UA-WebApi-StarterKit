import { IRequestMessage } from './IRequestMessage';
import { IResponseMessage } from './IResponseMessage';

export interface ICompletedRequest {
   callerHandle: number,
   request: IRequestMessage,
   response?: IResponseMessage,
   code?: number,
   message?: string
}
