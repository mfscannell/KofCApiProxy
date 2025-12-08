using KofCApiConsumer;
using KofCApiConsumer.KofCSdk;
using Serilog;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddSerilog(loggerConfiguration =>
{
    loggerConfiguration.ReadFrom.Configuration(builder.Configuration);
});
builder.Services.AddKofCService(builder.Configuration);
builder.Services.AddHostedService<Worker>();

var host = builder.Build();
host.Run();
