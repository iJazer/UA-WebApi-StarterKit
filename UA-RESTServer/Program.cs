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

using Microsoft.AspNetCore.Authentication;
using Microsoft.OpenApi.Models;
using Opc.Ua;
using Opc.Ua.Configuration;
using Ua.Rest.Server;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "OPC UA REST Server",
        Version = "v1",
        Description = "A REST-full interface for an OPC UA Server",
        Contact = new OpenApiContact
        {
            Name = "OPC Foundation",
            Email = "office@opcfoundation.org",
            Url = new Uri("https://opcfoundation.org/")
        }
    });

    options.AddSecurityDefinition("basic", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "basic",
        In = ParameterLocation.Header,
        Description = "Basic Authorization header using the Bearer scheme."
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "basic"
                }
            },
            Array.Empty<string>()
        }
    });

    options.CustomSchemaIds(type => type.ToString());

    options.EnableAnnotations();
});

builder.Services.AddAuthentication().AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("BasicAuthentication", null);

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

_ = Task.Run(() => ConsoleServer());

app.Run();

void ConsoleServer()
{
    ApplicationInstance.MessageDlg = new ApplicationMessageDlg();
    ApplicationInstance application = new ApplicationInstance();

    application.ApplicationName = "Opc.Ua.Station";
    application.ConfigSectionName = application.ApplicationName;
    application.ApplicationType = ApplicationType.Server;

    // load the application configuration
    ApplicationConfiguration appConfiguration = application.LoadApplicationConfiguration(false).Result;

    // check the application certificate
    bool certOK = application.CheckApplicationInstanceCertificate(false, 0).Result;
    if (!certOK)
    {
        throw new Exception("Application instance certificate invalid!");
    }

    // start the server
    application.Start(new FactoryStationServer());

    // wait forever
    Thread.Sleep(Timeout.Infinite);
}
