using KofCApiProxy.ApiProxy;
using KofCApiProxy.Middleware.Logging;
using KofCApiProxy.Options;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//var logger = new LoggerConfiguration()
//    .ReadFrom.Configuration(builder.Configuration)
//    .Enrich.FromLogContext()
//    .Enrich.With(new RemovePropertiesEnricher())
//    .CreateLogger();

//builder.Host.UseSerilog(logger);
// TODO MFS try using this as an alternative
builder.Host.UseSerilog((context, loggerConfig) =>
{
    loggerConfig.ReadFrom.Configuration(context.Configuration);
});
builder.Logging.ClearProviders();
// TODO MFS try with this commented out
//builder.Logging.AddSerilog(logger);

builder.Services.AddKofCService(builder.Configuration);
builder.Services.AddApiProxy();
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

// TODO MFS try with this below
app.UseSerilogRequestLogging();
app.UseHttpsRedirection();
app.UseMiddleware<RequestLogContextMiddleware>();

//app.UseAuthorization();

//app.MapControllers();

app.MapApiNugetAccounts();
app.MapApiNugetActivities();
app.MapApiNugetKnights();
app.MapApiProxy();

app.Run();
