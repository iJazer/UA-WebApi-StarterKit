using JWT;
using JWT.Algorithms;
using JWT.Builder;
using OAuthSampleClient;

Console.WriteLine("Loading OAuth2 location and client information.");
var settings = Settings.LoadSettings();

OAuthManager manager = new(settings);

Console.WriteLine("Requesting a JWT.");
var session = await manager.Login();

// validation is normally done inside the server that receives a JWT from a client.
await ValidateToken(manager, session);

Console.WriteLine("Cancelling the JWT.");
await manager.Logout(session);

Console.WriteLine("All done!");

static async Task ValidateToken(OAuthManager manager, Session session)
{
    Console.WriteLine("Fetching the keys needed to validate the JSON web token (JWT).");
    var keys = await manager.GetKeys();

    Console.WriteLine("Finding the key used to sign the JWT.");
    JwtHeader header = JwtBuilder.Create().DecodeHeader<JwtHeader>(session.AccessToken);

    string token;

    // need to disable time verification for now because the Wordpress OAuth2 plug-in inserts garbage data.
    var parameters = ValidationParameters.Default;
    parameters.ValidateExpirationTime = false;
    parameters.ValidateIssuedTime = false;

    if (keys.TryGetValue(header.KeyId, out var key))
    {
        token = JwtBuilder.Create()
            .WithAlgorithm(new RS256Algorithm(key))
            .WithValidationParameters(parameters)
            .Decode(session.AccessToken);

        Console.WriteLine("Signature is valid.");
    }
    else
    {
        token = JwtBuilder.Create()
            .DoNotVerifySignature()
            .Decode(session.AccessToken);

        Console.WriteLine("Signature could mot be verified!");
    }

    Console.WriteLine(new string('=', 80));
    Console.WriteLine(token);
    Console.WriteLine(new string('=', 80));
}
