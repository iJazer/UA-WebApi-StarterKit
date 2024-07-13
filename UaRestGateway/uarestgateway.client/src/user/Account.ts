import { MembershipType } from "./MembershipType"

export interface Account {
   name?: string,
   email?: string
   gitHubId?: string
   companyName?: string
   membershipType?: MembershipType,
   currentGitHubUser?: string
   accessToken?: string
}
