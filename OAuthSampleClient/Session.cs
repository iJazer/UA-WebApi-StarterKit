using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OAuthSampleClient
{
    public class Session
    {
        public string Id { get; set; }

        public string AccessToken { get; set; }

        public DateTime? ExpiresBy { get; set; }

        public string RefreshToken { get; set; }
    }
}
