using Newtonsoft.Json;
using System.Data;
using System.IO.Pipes;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text.Json.Serialization;

namespace OAuthSampleClient
{
    public class OAuthManager
    {
        private string BaseUrl = "https://opcfoundation.org";
        private string ClientId = "";
        private string ClientSecret = "";
        private string AuthorizeUrl = "{0}/oauth/authorize/";
        private string RevokeUrl = "{0}/oauth/revoke/";
        private string DestroyUrl = "{0}/oauth/destroy/";
        private string TokenUrl = "{0}/oauth/token/";
        private string MeUrl = "{0}/oauth/me/";
        private string KeysUrl = "{0}/.well-known/keys/";

        public OAuthManager(Settings settings)
        {
            BaseUrl = settings.OAuthServerUrl;
            ClientId = settings.OAuthClientId;
            ClientSecret = settings.OAuthClientSecret;
            AuthorizeUrl = String.Format(AuthorizeUrl, BaseUrl);
            RevokeUrl = String.Format(RevokeUrl, BaseUrl);
            DestroyUrl = String.Format(DestroyUrl, BaseUrl);
            TokenUrl = String.Format(TokenUrl, BaseUrl);
            MeUrl = String.Format(MeUrl, BaseUrl);
            KeysUrl = String.Format(KeysUrl, BaseUrl);
        }

        public async Task<Session> Login()
        {
            var form = new Dictionary<string, string>
            {
                { "grant_type", "client_credentials" },
                { "client_id", ClientId},
                { "client_secret", ClientSecret }
            };

            Session session = new Session();

            using (HttpClient client = CreateClient(TokenUrl))
            {
                var response = await client.PostAsync(TokenUrl, new FormUrlEncodedContent(form));
                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<Token>(json);

                session.AccessToken = result.AccessToken;
                session.RefreshToken = result.RefreshToken;
                session.ExpiresBy = DateTime.UtcNow.AddSeconds(result.ExpiresIn);
            }

            return session;
        }

        public async Task Logout(Session session)
        {
            if (session == null) throw new ArgumentNullException(nameof(session));

            var form = new Dictionary<string, string>
            {
                { "token", session.AccessToken }
            };

            using (HttpClient client = CreateClient(RevokeUrl))
            {
                session.AccessToken = null;
                session.RefreshToken = null;
                session.ExpiresBy = null;

                var response = await client.PostAsync(RevokeUrl, new FormUrlEncodedContent(form));
                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();

                session.AccessToken = null;
                session.ExpiresBy = null;
                session.RefreshToken = null;
            }
        }

        public async Task<Dictionary<string,RSA>> GetKeys()
        {
            using (HttpClient client = CreateClient(KeysUrl))
            {
                var response = await client.GetAsync(KeysUrl);
                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();
                var keys = JsonConvert.DeserializeObject<KeyList>(json);

                Dictionary<string, RSA> result = new();

                foreach (var key in keys.Keys)
                {
                    RSAParameters parameters = new RSAParameters();
                    parameters.Exponent = Convert.FromBase64String(key.Exponent);

                    var modulus = key.Modulus.Replace("-", "+").Replace("_", "/");

                    switch (modulus.Length % 3)
                    {
                        case 2: { modulus += "="; break; }
                        case 0: { modulus += "=="; break; }
                        default:
                        case 1: { break; }
                    }

                    var bytes = Convert.FromBase64String(modulus);
                    parameters.Modulus = bytes;
                    RSA rsa = RSA.Create();
                    rsa.ImportParameters(parameters);
                    result.Add(key.KeyId, rsa);
                }

                return result;
            }
        }

        public async Task Refresh(Session session)
        {
            if (session == null) throw new ArgumentNullException(nameof(session));

            var form = new Dictionary<string, string>
            {
                { "grant_type", "refresh_token" },
                { "refresh_token", $"{session.RefreshToken}" },
                { "client_id", ClientId },
                { "client_secret", ClientSecret }
            };

            using (HttpClient client = CreateClient(TokenUrl))
            {
                var response = await client.PostAsync(TokenUrl, new FormUrlEncodedContent(form));
                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<Token>(json);

                session.AccessToken = result.AccessToken;
                session.RefreshToken = result.RefreshToken;
                session.ExpiresBy = DateTime.UtcNow.AddSeconds(result.ExpiresIn);
            }
        }

        private HttpClient CreateClient(string url)
        {
            var handler = new HttpClientHandler();
            handler.ClientCertificateOptions = ClientCertificateOption.Manual;

            handler.ServerCertificateCustomValidationCallback =
                (httpRequestMessage, cert, cetChain, policyErrors) =>
                {
                    return true;
                };

            return new HttpClient(handler);
        }
    }

    internal class Token
    {
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }

        [JsonProperty("token_type")]
        public string TokenType { get; set; }

        [JsonProperty("expires_in")]
        public int ExpiresIn { get; set; }

        [JsonProperty("refresh_token")]
        public string RefreshToken { get; set; }
    }

    internal class UserInfo
    {
        [JsonProperty("ID")]
        public string Id { get; set; }

        [JsonProperty("display_name")]
        public string DisplayName { get; set; }

        [JsonProperty("user_login")]
        public string UserLogin { get; set; }

        [JsonProperty("user_nicename")]
        public string UserNiceName { get; set; }

        [JsonProperty("user_email")]
        public string UserEmail { get; set; }

        [JsonProperty("user_registered")]
        public DateTime? UserRegistered { get; set; }

        [JsonProperty("user_status")]
        public string UserStatus { get; set; }

        [JsonProperty("spam")]
        public string IsSpam { get; set; }

        [JsonProperty("deleted")]
        public string IsDeleted { get; set; }

        [JsonProperty("real_pass")]
        public string RealPass { get; set; }

        [JsonProperty("is_logged_in")]
        public string IsLoggedIn { get; set; }

        [JsonProperty("company_id")]
        public int? CompanyId { get; set; }

        [JsonProperty("company_name")]
        public string CompanyName { get; set; }

        [JsonProperty("membership_type")]
        public int? MembershipType { get; set; }
    }

    internal class KeyInfo
    {
        [JsonProperty("kid")]
        public string KeyId { get; set; }

        [JsonProperty("kty")]
        public string KeyType { get; set; }

        [JsonProperty("alg")]
        public string Algorithm { get; set; }

        [JsonProperty("sig")]
        public string Usage { get; set; }

        [JsonProperty("n")]
        public string Modulus { get; set; }

        [JsonProperty("e")]
        public string Exponent { get; set; }
    }


    internal class KeyList
    {
        [JsonProperty("keys")]
        public List<KeyInfo> Keys { get; set; }
    }
}
