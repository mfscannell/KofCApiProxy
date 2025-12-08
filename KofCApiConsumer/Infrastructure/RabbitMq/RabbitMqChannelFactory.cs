using Microsoft.AspNetCore.Mvc.ModelBinding;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KofCApiConsumer.Infrastructure.RabbitMq;

internal interface IRabbitMqChannelFactory
{
    Task<IChannel> Create(IConnection connection);
}
internal class RabbitMqChannelFactory : IRabbitMqChannelFactory
{
    public async Task<IChannel> Create(IConnection connection)
    {
        var channel = await connection.CreateChannelAsync();
        channel.ChannelShutdownAsync += ChannelOnShutdownAsync;
        channel.CallbackExceptionAsync += Channel_CallbackExceptionAsync;
        await channel.BasicQosAsync(prefetchSize: 0, prefetchCount: 120, global: false);

        return channel;
    }

    private async Task Channel_CallbackExceptionAsync(
        object sender,
        CallbackExceptionEventArgs @event)
    {
    }

    private async Task ChannelOnShutdownAsync(
        object? sender,
        ShutdownEventArgs shutdownEventArgs)
    {

    }
}
