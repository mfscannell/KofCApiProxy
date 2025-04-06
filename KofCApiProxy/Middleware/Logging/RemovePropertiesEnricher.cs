using Serilog.Core;
using Serilog.Events;

namespace KofCApiProxy.Middleware.Logging;

public class RemovePropertiesEnricher : ILogEventEnricher
{
    public void Enrich(LogEvent le, ILogEventPropertyFactory lepf)
    {
        le.RemovePropertyIfPresent("ActionId");
        le.RemovePropertyIfPresent("RequestId");
        //le.RemovePropertyIfPresent("Password");
        //le.RemovePropertyIfPresent("ConfirmPassword");
        //le.RemovePropertyIfPresent("OldPassword");
        //le.RemovePropertyIfPresent("NewPassword");
    }
}
