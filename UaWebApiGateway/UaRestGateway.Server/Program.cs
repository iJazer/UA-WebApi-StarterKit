using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Globalization;
using System.Security.Claims;
using System.Text.Json;
using UaRestGateway.Server.Model;
using UaRestGateway.Server.Service;
using UaRestGateway.Server.Service.AAS;

namespace UaRestGateway.Server
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllers();

            builder.Services.AddSingleton<IUACommunicationService, UACommunicationService>();
            builder.Services.AddHostedService<UACommunicationService>(provider => (UACommunicationService)provider.GetService<IUACommunicationService>());

            builder.Services.AddSingleton<IAASCommunicationService, AASCommunicationService>();
            builder.Services.AddHostedService(sp => (AASCommunicationService)sp.GetService<IAASCommunicationService>());

            builder.Services.AddScoped<IAasTreeService, AasTreeService>();
            builder.Services.AddTransient<IBase64UrlDecoderService, Base64UrlDecoderService>();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
    
            builder.Services.AddMemoryCache(a =>
            {
                a.ExpirationScanFrequency = TimeSpan.FromSeconds(600);
            });

            builder.Services.AddDbContext<DatabaseContext>(options => DatabaseContext.GetOptions(
                builder.Configuration.GetConnectionString("DatabaseContext"),
                options));

            builder.Services.AddDistributedMemoryCache();

            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromSeconds(3600);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            builder.Services.AddCors(options =>
            {
                options.AddDefaultPolicy(
                    builder => builder
                        .WithOrigins("http://localhost:5173")
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials()
                );
            });

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
            {
                options.Events = new JwtBearerEvents()
                {
                    OnMessageReceived = context =>
                    {
                        var cache = context.HttpContext.RequestServices.GetRequiredService<IMemoryCache>();

                        context.Options.TokenValidationParameters.IssuerSigningKeyResolver = (s, securityToken, identifier, parameters) =>
                        {
                            if (!cache.TryGetValue(nameof(TokenValidationParameters.IssuerSigningKey), out JsonWebKey key))
                            {
                                using (var client = new HttpClient())
                                {
                                    client.DefaultRequestHeaders.UserAgent.ParseAdd(UserAgents.Default);
                       
                                    var response = client.GetAsync(parameters.ValidIssuer + "/.well-known/keys").ConfigureAwait(false).GetAwaiter().GetResult();
                                    response.EnsureSuccessStatusCode();
                                    
                                    string json = response.Content.ReadAsStringAsync().ConfigureAwait(false).GetAwaiter().GetResult();
                                    var keyset = new JsonWebKeySet(json);
                                    key = keyset.Keys.FirstOrDefault();

                                    cache.Set(
                                        nameof(TokenValidationParameters.IssuerSigningKey), 
                                        key,
                                        new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromHours(1)));
                                }
                            }

                            return new[] { key };
                        };

                        return Task.CompletedTask;
                    }
                };

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = false,
                    ValidIssuer = "https://opcfoundation.org"
                };
            });

            var app = builder.Build();

            app.UseDefaultFiles();
            
            app.Use(async (context, next) =>
            {
                if (context.Request.Path == "/data/opc.ua.openapi.allservices.json")
                {
                    context.Response.Headers.Append("Access-Control-Allow-Origin", "*");
                    context.Response.Headers.Append("Access-Control-Allow-Methods", "GET");
                    context.Response.Headers.Append("Access-Control-Allow-Headers", "Content-Type");
                }
                await next();
            });

            app.UseStaticFiles();
            
            app.UseSession();
            app.UseWebSockets();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseCors();
            app.UseAuthorization();
            app.UseAuthentication();
            app.MapControllers();
            app.MapFallbackToFile("/index.html");

            app.Run();
        }
    }
}
