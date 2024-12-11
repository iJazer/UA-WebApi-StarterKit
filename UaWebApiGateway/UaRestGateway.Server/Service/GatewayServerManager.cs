/* ========================================================================
 * Copyright (c) 2005-2020 The OPC Foundation, Inc. All rights reserved.
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

using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.IdentityModel.Tokens;
using Opc.Ua;
using Opc.Ua.Bindings;
using Opc.Ua.Server;
using UaRestGateway.Server.Model;
using StatusCodes = Opc.Ua.StatusCodes;

namespace UaRestGateway.Server.Service
{
    public partial class GatewayServerManager : StandardServer
    {
        #region Private Fields
        private ICertificateValidator m_userCertificateValidator;
        #endregion

        public GatewayServerManager()
        {
        }

        #region Overridden Methods
        protected override IList<ServiceHost> InitializeServiceHosts(
            ApplicationConfiguration configuration,
            TransportListenerBindings bindingFactory,
            out ApplicationDescription serverDescription,
            out EndpointDescriptionCollection endpoints)
        {
            var hosts = base.InitializeServiceHosts(configuration, bindingFactory, out serverDescription, out endpoints);

            var templateEndpoint = endpoints.Where(x => x.SecurityMode == MessageSecurityMode.None).FirstOrDefault();

            endpoints.Add(new EndpointDescription()
            {
                EndpointUrl = "opc.tcp://localhost:4840",
                SecurityMode = MessageSecurityMode.None,
                SecurityPolicyUri = SecurityPolicies.None,
                TransportProfileUri = Profiles.HttpsJsonTransport,
                SecurityLevel = 100,
                Server = serverDescription,
                ServerCertificate = templateEndpoint.ServerCertificate,
                UserIdentityTokens = templateEndpoint.UserIdentityTokens.Where(x => x.TokenType == UserTokenType.IssuedToken || x.TokenType == UserTokenType.UserName || x.TokenType == UserTokenType.Anonymous).ToArray()
            });

            return hosts;
        }

        protected override MasterNodeManager CreateMasterNodeManager(IServerInternal server, ApplicationConfiguration configuration)
        {
            Utils.Trace("Creating the Node Managers.");

            List<INodeManager> nodeManagers = new List<INodeManager>
            {
                new GatewayNodeManager(server, configuration)
            };

            // create master node manager.
            return new MasterNodeManager(server, configuration, null, nodeManagers.ToArray());
        }

        protected override ServerProperties LoadServerProperties()
        {
            ServerProperties properties = new ServerProperties();
  
            properties.ManufacturerName = this.GetType().Assembly.GetCustomAttributes<AssemblyCompanyAttribute>().FirstOrDefault()?.Company;
            properties.ProductName = this.GetType().Assembly.GetCustomAttributes<AssemblyProductAttribute>().FirstOrDefault()?.Product;
            properties.ProductUri = this.Configuration.ProductUri;
            properties.SoftwareVersion = Utils.GetAssemblySoftwareVersion();
            properties.BuildNumber = Utils.GetAssemblyBuildNumber();
            properties.BuildDate = Utils.GetAssemblyTimestamp();

            return properties;
        }

        protected override ResourceManager CreateResourceManager(IServerInternal server, ApplicationConfiguration configuration)
        {
            ResourceManager resourceManager = new ResourceManager(server, configuration);

            System.Reflection.FieldInfo[] fields = typeof(StatusCodes).GetFields(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);

            foreach (System.Reflection.FieldInfo field in fields)
            {
                uint? id = field.GetValue(typeof(StatusCodes)) as uint?;

                if (id != null)
                {
                    resourceManager.Add(id.Value, "en-US", field.Name);
                }
            }

            return resourceManager;
        }

        protected override void OnServerStarting(ApplicationConfiguration configuration)
        {
            Utils.Trace("The server is starting.");

            base.OnServerStarting(configuration);

            // it is up to the application to decide how to validate user identity tokens.
            // this function creates validator for X509 identity tokens.
            CreateUserIdentityValidators(configuration);
        }

        protected override void OnServerStarted(IServerInternal server)
        {
            base.OnServerStarted(server);

            server.DiagnosticsNodeManager.DeleteNode(server.DefaultSystemContext, ObjectIds.Aliases);
            server.DiagnosticsNodeManager.DeleteNode(server.DefaultSystemContext, ObjectIds.Locations);
            server.DiagnosticsNodeManager.DeleteNode(server.DefaultSystemContext, ObjectIds.Quantities);

            // request notifications when the user identity is changed. all valid users are accepted by default.
            server.SessionManager.ImpersonateUser += new ImpersonateEventHandler(SessionManager_ImpersonateUser);
        }
        #endregion

        #region User Validation Functions
        /// <summary>
        /// Creates the objects used to validate the user identity tokens supported by the server.
        /// </summary>
        private void CreateUserIdentityValidators(ApplicationConfiguration configuration)
        {
            for (int ii = 0; ii < configuration.ServerConfiguration.UserTokenPolicies.Count; ii++)
            {
                UserTokenPolicy policy = configuration.ServerConfiguration.UserTokenPolicies[ii];

                // create a validator for a certificate token policy.
                if (policy.TokenType == UserTokenType.Certificate)
                {
                    // check if user certificate trust lists are specified in configuration.
                    if (configuration.SecurityConfiguration.TrustedUserCertificates != null &&
                        configuration.SecurityConfiguration.UserIssuerCertificates != null)
                    {
                        CertificateValidator certificateValidator = new CertificateValidator();
                        certificateValidator.Update(configuration.SecurityConfiguration).Wait();
                        certificateValidator.Update(configuration.SecurityConfiguration.UserIssuerCertificates,
                            configuration.SecurityConfiguration.TrustedUserCertificates,
                            configuration.SecurityConfiguration.RejectedCertificateStore);

                        // set custom validator for user certificates.
                        m_userCertificateValidator = certificateValidator.GetChannelValidator();
                    }
                }
            }
        }

        private async Task<UserIdentity> Validate(IssuedIdentityToken token)
        {
            var jwtToken = new UTF8Encoding(false).GetString(token.DecryptedTokenData);

            var parameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = false,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = false,
                ValidIssuer = "https://opcfoundation.org",
                ClockSkew = TimeSpan.FromMinutes(15)
            };

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.UserAgent.ParseAdd(UserAgents.Default);

                var response = await client.GetAsync(parameters.ValidIssuer + "/.well-known/keys").ConfigureAwait(false);
                response.EnsureSuccessStatusCode();

                string json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                var keyset = new JsonWebKeySet(json);
                var key = keyset.Keys.FirstOrDefault(); 
                var tokenHandler = new JwtSecurityTokenHandler();

                parameters.IssuerSigningKeyResolver = (s, securityToken, identifier, parameters) => new[] { key };

                try
                {
                    SecurityToken validatedToken;
                    var claimsPrincipal = tokenHandler.ValidateToken(jwtToken, parameters, out validatedToken);
                    Console.WriteLine(validatedToken.ToString());

                    response = await client.GetAsync($"{parameters.ValidIssuer}/oauth/me/?access_token={jwtToken}");
                    response.EnsureSuccessStatusCode();

                    json = await response.Content.ReadAsStringAsync();
                    var body = JsonSerializer.Deserialize<UserInfo>(json);

                    var identity = new UserIdentity(token)
                    {
                         DisplayName = body.UserEmail
                    };

                    identity.GrantedRoleIds.Add(ObjectIds.WellKnownRole_AuthenticatedUser);

                    if (UserInfo.GetMembershipType(body.MembershipType) == MembershipType.Corporate)
                    {
                        identity.GrantedRoleIds.Add(ObjectIds.WellKnownRole_Engineer);
                    }

                    return identity;
                }
                catch (Exception e)
                {
                    throw new Exception("Invalid issuer signing key", e);
                }
            }
        }

        /// <summary>
        /// Called when a client tries to change its user identity.
        /// </summary>
        private void SessionManager_ImpersonateUser(Session session, ImpersonateEventArgs args)
        {
            // check for issued token.
            if (args.NewIdentity is IssuedIdentityToken issuedToken)
            {
                if (issuedToken.IssuedTokenType == IssuedTokenType.JWT)
                {
                    args.Identity = Validate(issuedToken).ConfigureAwait(false).GetAwaiter().GetResult();
                    return;
                }
            }

            // check for a user name token.
            UserNameIdentityToken userNameToken = args.NewIdentity as UserNameIdentityToken;

            if (userNameToken != null)
            {
                args.Identity = VerifyPassword(userNameToken);

                // set AuthenticatedUser role for accepted user/password authentication
                args.Identity.GrantedRoleIds.Add(ObjectIds.WellKnownRole_AuthenticatedUser);

                if (args.Identity is SystemConfigurationIdentity)
                {
                    // set ConfigureAdmin role for user with permission to configure server
                    args.Identity.GrantedRoleIds.Add(ObjectIds.WellKnownRole_ConfigureAdmin);
                    args.Identity.GrantedRoleIds.Add(ObjectIds.WellKnownRole_SecurityAdmin);
                }

                return;
            }

            // check for x509 user token.
            X509IdentityToken x509Token = args.NewIdentity as X509IdentityToken;

            if (x509Token != null)
            {
                VerifyUserTokenCertificate(x509Token.Certificate);
                args.Identity = new UserIdentity(x509Token);
                Utils.Trace("X509 Token Accepted: {0}", args.Identity.DisplayName);

                // set AuthenticatedUser role for accepted certificate authentication
                args.Identity.GrantedRoleIds.Add(ObjectIds.WellKnownRole_AuthenticatedUser);

                return;
            }

            // check for anonymous token.
            if (args.NewIdentity is AnonymousIdentityToken || args.NewIdentity == null)
            {
                // allow anonymous authentication and set Anonymous role for this authentication
                args.Identity = new UserIdentity();
                args.Identity.GrantedRoleIds.Add(ObjectIds.WellKnownRole_Anonymous);

                return;
            }

            // unsuported identity token type.
            throw ServiceResultException.Create(StatusCodes.BadIdentityTokenInvalid,
                   "Not supported user token type: {0}.", args.NewIdentity);
        }

        /// <summary>
        /// Validates the password for a username token.
        /// </summary>
        private IUserIdentity VerifyPassword(UserNameIdentityToken userNameToken)
        {
            switch (userNameToken.UserName)
            {
                case "user1":
                    if (userNameToken.DecryptedPassword != "password1")
                    {
                        throw new ServiceResultException(StatusCodes.BadIdentityTokenRejected);
                    }
                    break;

                case "user2":
                    if (userNameToken.DecryptedPassword != "password2")
                    {
                        throw new ServiceResultException(StatusCodes.BadIdentityTokenRejected);
                    }
                    break;

                default:
                    throw new ServiceResultException(StatusCodes.BadIdentityTokenRejected);
            }

            return new UserIdentity(userNameToken);
        }

        /// <summary>
        /// Verifies that a certificate user token is trusted.
        /// </summary>
        private void VerifyUserTokenCertificate(X509Certificate2 certificate)
        {
            try
            {
                if (m_userCertificateValidator != null)
                {
                    m_userCertificateValidator.Validate(certificate);
                }
                else
                {
                    CertificateValidator.Validate(certificate);
                }
            }
            catch (Exception e)
            {
                TranslationInfo info;
                StatusCode result = StatusCodes.BadIdentityTokenRejected;
                ServiceResultException se = e as ServiceResultException;
                if (se != null && se.StatusCode == StatusCodes.BadCertificateUseNotAllowed)
                {
                    info = new TranslationInfo(
                        "InvalidCertificate",
                        "en-US",
                        "'{0}' is an invalid user certificate.",
                        certificate.Subject);

                    result = StatusCodes.BadIdentityTokenInvalid;
                }
                else
                {
                    // construct translation object with default text.
                    info = new TranslationInfo(
                        "UntrustedCertificate",
                        "en-US",
                        "'{0}' is not a trusted user certificate.",
                        certificate.Subject);
                }

                // create an exception with a vendor defined sub-code.
                throw new ServiceResultException(new ServiceResult(
                    result,
                    info.Key,
                    LoadServerProperties().ProductUri,
                    new LocalizedText(info)));
            }
        }
        #endregion
    }
}
