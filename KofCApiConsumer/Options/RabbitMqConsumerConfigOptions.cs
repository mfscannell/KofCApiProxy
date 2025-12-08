namespace KofCApiConsumer.Options;

internal class RabbitMqConsumerConfigOptions
{
    public string Exchange { get; set; } = string.Empty;
    public string Queue { get; set; } = string.Empty;
}
