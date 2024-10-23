export enum ReleaseStatus {
   Invalid = 0,
   Draft = 1,
   ReleaseCandidate = 2,
   Released = 3,
   Deprecated = 4
}

export interface ErrorInfo {
   errorCode?: string
   errorText?: string
   silent?: boolean
   failed?: boolean
}

export interface BaseRecord {
   id?: number
}

export interface NamedRecord extends BaseRecord {
   name?: string
}

export interface ProfileSet extends NamedRecord {
   profileGroupId?: number
   releaseStatus?: ReleaseStatus
   expiryDate?: string
}

export interface NodeSet extends NamedRecord {
   modelVersion: string
   publicationDate: string
   expiryDate?: string
}

export enum TaskStatus {
   Created = 0,
   Ready = 1,
   Running = 2,
   Completed = 3
}

export interface Task extends NamedRecord {
   modelUri?: string
   fileName?: string
   errorsToSuppress?: string
   patternsToIgnore?: string
   checkConformanceUnits?: boolean
   sendNotifications?: boolean
   lastActivityTime?: string
   expiryDate?: string,
   status?: TaskStatus,
   apiKey?: string
}

export enum TaskResultSeverity {
   Unknown = 0,
   Error = 1,
   Warning = 2,
   Control = 3,
   Information = 4
}

export interface TaskResult extends BaseRecord {
   time?: string
   severity?: TaskResultSeverity
   message?: string
}
