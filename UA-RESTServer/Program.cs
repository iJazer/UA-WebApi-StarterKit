using Opc.Ua;
using Opc.Ua.Configuration;
using Ua.Rest.Server;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

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
