using System.IO.Compression;
using System.Text;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore.Sqlite.Query.Internal;
using Opc.Ua;
using Opc.Ua.Server;
using UaRestGateway.Server.Service;

namespace UaRestGateway.Server.Controllers
{
    public class Token
    {
        [JsonPropertyName("access_token")]
        public string AccessToken { get; set; }

        [JsonPropertyName("token_type")]
        public string TokenType { get; set; }

        [JsonPropertyName("expires_in")]
        public int ExpiresIn { get; set; }

        [JsonPropertyName("refresh_token")]
        public string RefreshToken { get; set; }
    }

    public class CacheSessions
    {
        public string SecureChannelId { get; set; }

        public Dictionary<string, string> AuthenticationTokens { get; set; }
    }
}
