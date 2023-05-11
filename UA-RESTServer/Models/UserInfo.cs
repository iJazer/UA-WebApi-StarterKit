/* ========================================================================
 * Copyright (c) 2005-2023 The OPC Foundation, Inc. All rights reserved.
 *
 * OPC Foundation MIT License 1.00
 *
 * Permission is hereby granted, free of charge, to any person
 * obtaining a copy of this software and associated documentation
 * files (the "Software"), to deal in the Software without
 * restriction, including without limitation the rights to use,
 * copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the
 * Software is furnished to do so, subject to the following
 * conditions:
 *
 * The above copyright notice and this permission notice shall be
 * included in all copies or substantial portions of the Software.
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
 * EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
 * OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
 * NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
 * HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
 * WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
 * FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
 * OTHER DEALINGS IN THE SOFTWARE.
 *
 * The complete license agreement can be found here:
 * http://opcfoundation.org/License/MIT/1.00/
 * ======================================================================*/

using Newtonsoft.Json;

namespace UA_RESTServer.Models
{
    internal class UserInfo
    {
        [JsonProperty("ID")]
        public string Id { get; set; } = string.Empty;

        [JsonProperty("display_name")]
        public string DisplayName { get; set; } = string.Empty;

        [JsonProperty("user_login")]
        public string UserLogin { get; set; } = string.Empty;

        [JsonProperty("user_nicename")]
        public string UserNiceName { get; set; } = string.Empty;

        [JsonProperty("user_email")]
        public string UserEmail { get; set; } = string.Empty;

        [JsonProperty("user_registered")]
        public DateTime? UserRegistered { get; set; }

        [JsonProperty("user_status")]
        public string UserStatus { get; set; } = string.Empty;

        [JsonProperty("spam")]
        public string IsSpam { get; set; } = string.Empty;

        [JsonProperty("deleted")]
        public string IsDeleted { get; set; } = string.Empty;

        [JsonProperty("real_pass")]
        public string RealPass { get; set; } = string.Empty;

        [JsonProperty("is_logged_in")]
        public string IsLoggedIn { get; set; } = string.Empty;

        [JsonProperty("company_id")]
        public int? CompanyId { get; set; }

        [JsonProperty("company_name")]
        public string CompanyName { get; set; } = string.Empty;

        [JsonProperty("membership_type")]
        public int? MembershipType { get; set; }
    }
}
