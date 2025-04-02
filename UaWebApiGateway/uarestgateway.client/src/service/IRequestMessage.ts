import { IRequestBody } from './IRequestBody';

export interface IRequestMessage {
   CallerId?: string,
   ServiceId?: string,
   LocaleIds?: string[],
   Body: IRequestBody
}