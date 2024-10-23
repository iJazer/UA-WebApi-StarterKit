import { MembershipType } from "./MembershipType"

export interface Account {
   id?: number,
   name?: string,
   email?: string
   gitHubId?: string
   companyName?: string
   membershipType?: MembershipType,
   currentGitHubUser?: string
   accessToken?: string
}
