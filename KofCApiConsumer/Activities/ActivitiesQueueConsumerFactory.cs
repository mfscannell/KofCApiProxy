using KofCApiConsumer.Infrastructure.RabbitMq;
using KofCApiConsumer.Options;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KofCApiConsumer.Activities;

internal interface IActivitiesQueueConsumerFactory
{
    IActivitiesQueueConsumer Create(IConnection connection);
}

internal class ActivitiesQueueConsumerFactory : IActivitiesQueueConsumerFactory
{
    private readonly ILogger<ActivitiesQueueConsumerFactory> _logger;
    private readonly IRabbitMqChannelFactory _rabbitMqChannelFactory;
    private readonly IOptions<RabbitMqConsumerConfigOptions> _rabbitMqConsumerConfigOptions;
    private readonly IEventBridgeForwardingService _eventBridgeForwardingService;
    public IActivitiesQueueConsumer Create(IConnection connection)
    {
        throw new NotImplementedException();
    }
}
