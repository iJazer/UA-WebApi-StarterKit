import { IRequestBody } from './IRequestBody';

export interface IRequestMessage {
   ServiceId?: string,
   LocaleIds?: string[],
   Body: IRequestBody
}