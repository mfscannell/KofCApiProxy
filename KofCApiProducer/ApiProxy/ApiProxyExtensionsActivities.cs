using KofCApiProducer.Options;
using KofCSDK;
using KofCSDK.Models.Requests;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace KofCApiProducer.ApiProxy;

public static class ApiProxyExtensionsActivities
{
    public static void MapApiActivities(this WebApplication app)
    {
        app.MapPost("api/{tenantId}/{version}/activities", async (
            HttpContext context,
            [FromRoute] string tenantId,
            [FromBody] CreateActivityRequest request,
            IOptions<RabbitMqConnectionConfigOptions> rabbitMqConnectionConfigOptions,
            CancellationToken cancellationToken) =>
        {
            var rabbitMqConnectionConfig = rabbitMqConnectionConfigOptions.Value;
            var tenantInfo = new TenantInfo
            {
                TenantId = tenantId
            };
            var token = context.Request.Headers.Authorization.ToString();

            if (!string.IsNullOrWhiteSpace(token) && token.StartsWith("Bearer "))
            {
                token = token.Substring(7);
            }

            var factory = new ConnectionFactory
            {
                HostName = rabbitMqConnectionConfig.HostName,
                Port = rabbitMqConnectionConfig.Port,
                UserName = rabbitMqConnectionConfig.Username,
                Password = rabbitMqConnectionConfig.Password
            };

            using var connection = await factory.CreateConnectionAsync();
            using var channel = await connection.CreateChannelAsync();

            await channel.QueueDeclareAsync(
                queue: rabbitMqConnectionConfig.CreateActivitiesQueue,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null);
            var authenticatedTenantRequest = new AuthenticatedTenantRequest<CreateActivityRequest>
            {
                Payload = request,
                TenantInfo = tenantInfo,
                UserAuthentication = new UserAuthentication
                {
                    WebToken = token
                }
            };
            var jsonString = JsonSerializer.Serialize(authenticatedTenantRequest);
            var body = Encoding.UTF8.GetBytes(jsonString);

            app.Logger.LogInformation("Publishing CreateActivityRequest to queue {@CreateActivityRequest}", authenticatedTenantRequest);

            await channel.BasicPublishAsync(
                exchange: string.Empty,
                routingKey: rabbitMqConnectionConfig.CreateActivitiesQueue,
                mandatory: true,
                basicProperties: new BasicProperties { Persistent = true },
                body: body);

            return Results.Ok();
        })
            .Produces<KofCSDK.Models.Responses.Activity>(StatusCodes.Status200OK)
            .Produces<KofCSDK.Models.ProblemDetails>(StatusCodes.Status500InternalServerError);
    }
}
