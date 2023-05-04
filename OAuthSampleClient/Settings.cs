using Microsoft.Extensions.Configuration;

namespace OAuthSampleClient
{
    public class Settings
    {
        public string OAuthServerUrl { get; set; }

        public string OAuthClientId { get; set; }

        public string OAuthClientSecret { get; set; }

        public static Settings LoadSettings()
        {
            // Load settings
            IConfiguration config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false)
                .AddUserSecrets<Settings>()
                .Build();

            return config.GetRequiredSection("Settings").Get<Settings>();
        }
    }
}
