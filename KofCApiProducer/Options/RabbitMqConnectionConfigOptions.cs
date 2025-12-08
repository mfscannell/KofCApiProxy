namespace KofCApiProducer.Options;

public class RabbitMqConnectionConfigOptions
{
    public const string SectionName = "RabbitMqConnectionConfig";
    public string HostName { get; set; } = string.Empty;
    public string CreateActivitiesQueue {  get; set; } = string.Empty;
    public int Port { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}
