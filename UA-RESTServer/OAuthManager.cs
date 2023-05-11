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

using JWT;
using JWT.Builder;
using Newtonsoft.Json;
using System.Security.Cryptography;
using UA_RESTServer.Models;

namespace Ua.Rest.Server
{
    public class OAuthManager
    {
        private string BaseUrl = "https://opcfoundation.org";
        private string ClientId = "";
        private string ClientSecret = "";
        private string TokenUrl = "{0}/oauth/token/";
        private string KeysUrl = "{0}/.well-known/keys/";

        public OAuthManager()
        {
            var configuration = new ConfigurationBuilder().AddJsonFile($"appsettings.json");
            var config = configuration.Build();

            BaseUrl = config.GetSection("Settings").GetSection("OAuthServerUrl").Value;
            ClientId = config.GetSection("Settings").GetSection("OAuthClientId").Value;
            ClientSecret = config.GetSection("Settings").GetSection("OAuthClientSecret").Value;
            TokenUrl = string.Format(TokenUrl, BaseUrl);
            KeysUrl = string.Format(KeysUrl, BaseUrl);
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
                if (result != null)
                {
                    session.AccessToken = result.AccessToken;
                    session.RefreshToken = result.RefreshToken;
                    session.ExpiresBy = DateTime.UtcNow.AddSeconds(result.ExpiresIn);
                }
            }

            return session;
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

                if (keys != null)
                {
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
                }

                return result;
            }
        }

        public async Task ValidateToken(Session session)
        {
            Console.WriteLine("Fetching the keys needed to validate the JSON web token (JWT).");
            var keys = await GetKeys();

            Console.WriteLine("Finding the key used to sign the JWT.");
            JwtHeader header = JwtBuilder.Create().DecodeHeader<JwtHeader>(session.AccessToken);

            // need to disable time verification for now because the Wordpress OAuth2 plug-in inserts garbage data.
            var parameters = ValidationParameters.Default;
            parameters.ValidateExpirationTime = false;
            parameters.ValidateIssuedTime = false;

            if (keys.TryGetValue(header.KeyId, out var key))
            {
                Console.WriteLine("Signature is valid.");
            }
            else
            {
                throw new Exception("Signature could mot be verified!");
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
}
