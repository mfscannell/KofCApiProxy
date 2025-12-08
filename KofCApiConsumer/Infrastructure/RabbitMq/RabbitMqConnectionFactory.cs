using KofCApiConsumer.Options;
using Microsoft.Extensions.Options;
using Polly;
using Polly.Retry;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace KofCApiConsumer.Infrastructure.RabbitMq;

internal interface IRabbitMqConnectionFactory
{
    Task<IConnection?> CreateAsync();
}

internal class RabbitMqConnectionFactory : IRabbitMqConnectionFactory
{
    private readonly ILogger<RabbitMqConnectionFactory> _logger;
    private readonly RabbitMqConnectionConfigOptions _rabbitMqConnectionConfigOptions;

    public RabbitMqConnectionFactory(
        ILogger<RabbitMqConnectionFactory> logger,
        IOptions<RabbitMqConnectionConfigOptions> rabbitMqConnectionConfigOptions)
    {
        _logger = logger;
        _rabbitMqConnectionConfigOptions = rabbitMqConnectionConfigOptions.Value;
    }
    public async Task<IConnection?> CreateAsync()
    {
        var connectionFactory = new ConnectionFactory()
        {
            HostName = _rabbitMqConnectionConfigOptions.HostName,
            UserName = _rabbitMqConnectionConfigOptions.UserName,
            Password = _rabbitMqConnectionConfigOptions.Password,
            Port = _rabbitMqConnectionConfigOptions.Port,
            VirtualHost = _rabbitMqConnectionConfigOptions.VirtualHost,
            ClientProvidedName = "something"
        };
        var retryPolicy = CreateRetryPolicy();
        var policyResult = retryPolicy.ExecuteAndCapture(() => connectionFactory.CreateConnectionAsync());

        if (policyResult.Outcome != OutcomeType.Successful)
        {
            _logger.LogError("Could not create RabbitMQ connection after multiple attempts.");
            return null;
        }

        var connection = await policyResult.Result;
        connection.ConnectionShutdownAsync += ConnectionOnConnectionShutdownAsync;
        connection.CallbackExceptionAsync += ConnectionOnCallbackExceptionAsync;

        return connection;
    }

    private async Task ConnectionOnConnectionShutdownAsync(object sender, ShutdownEventArgs @event)
    {
        var logLevel = @event is { ReplyCode: 200, ReplyText: "Goodbye" } ? LogLevel.Information : LogLevel.Error;
        _logger.Log(logLevel, "RabbitMQ connection shutdown occurred. {@ShutdownEventArgs}", @event);
    }

    private async Task ConnectionOnCallbackExceptionAsync(object sender, CallbackExceptionEventArgs @event)
    {
        _logger.LogError(@event.Exception, "RabbitMQ connection callback exception occurred.");
    }

    private RetryPolicy CreateRetryPolicy()
    {
        const int retryCount = 5;

        return Policy
            .Handle<Exception>()
            .WaitAndRetry(
                retryCount,
                retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                (exception, timeSpan, retryAttempt, context) =>
                {
                    _logger.LogWarning(exception, "Attempt {RetryAttempt} to connect to RabbitMQ failed. Waiting {TimeSpan} before next retry.", retryAttempt, timeSpan);
                });
    }
}
