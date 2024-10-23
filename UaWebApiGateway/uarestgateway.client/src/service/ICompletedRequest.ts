import { IRequestMessage } from './IRequestMessage';
import { IResponseMessage } from './IResponseMessage';

export interface ICompletedRequest {
   clientHandle: number,
   request: IRequestMessage,
   response?: IResponseMessage,
   code?: number,
   message?: string
}
