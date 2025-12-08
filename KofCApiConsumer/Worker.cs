using KofCApiConsumer.Activities;
using KofCApiConsumer.Infrastructure.RabbitMq;
using RabbitMQ.Client;

namespace KofCApiConsumer;

internal class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly IHostApplicationLifetime _hostApplicationLifetime;
    private readonly IRabbitMqConnectionFactory _rabbitMqConnectionFactory;
    private IConnection? _connection;
    private readonly IActivitiesQueueConsumerFactory _activitiesQueueConsumerFactory;
    private IActivitiesQueueConsumer? _activitiesQueueConsumer;

    public Worker(
        ILogger<Worker> logger,
        IHostApplicationLifetime hostApplicationLifetime,
        IRabbitMqConnectionFactory rabbitMqConnectionFactory,
        IActivitiesQueueConsumerFactory activitiesQueueConsumerFactory)
    {
        _logger = logger;
        _hostApplicationLifetime = hostApplicationLifetime;
        _rabbitMqConnectionFactory = rabbitMqConnectionFactory;
        _activitiesQueueConsumerFactory = activitiesQueueConsumerFactory;
    }

    public override async Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogDebug("Worker starting.");

        _connection = await _rabbitMqConnectionFactory.CreateAsync();

        if (_connection is null)
        {
            _logger.LogCritical("Failed to create RabbitMQ connection. Stopping application.");
            _hostApplicationLifetime.StopApplication();
            return;
        }

        _activitiesQueueConsumer = _activitiesQueueConsumerFactory.Create(_connection);

        await base.StartAsync(cancellationToken);
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            if (_logger.IsEnabled(LogLevel.Information))
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
            }
            await Task.Delay(1000, stoppingToken);
        }
    }
}
