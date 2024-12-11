namespace UaRestGateway.Server.Model
{
    public static class ErrorCodes
    {
        public const string UnexpectedError = "An unexpected error occurred.";

        public const string NotLoggedIn = "No user is logged in.";

        public const string UserNotRegistered = "The user has not registered with the application.";

        public static string ServerNotRunning = "The server is not running.";
    }

    public static class UserAgents
    {
        public const string Default = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/116.0.0.0 Safari/537.36";
    }
}
