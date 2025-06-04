using KofCApiProxy.ApiProxy;
using KofCApiProxy.Middleware.Logging;
using KofCApiProxy.Options;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, loggerConfig) =>
{
    loggerConfig.ReadFrom.Configuration(context.Configuration);
});
builder.Logging.ClearProviders();
builder.Services.AddKofCService(builder.Configuration);
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

app.UseSerilogRequestLogging();
app.UseHttpsRedirection();
app.UseMiddleware<RequestLogContextMiddleware>();

app.MapApiNugetAccounts();
app.MapApiNugetActivities();
app.MapApiNugetKnights();

app.Run();
