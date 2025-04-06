using KofCSDK;
using Microsoft.Extensions.Options;

namespace KofCApiProxy.Options;

public static class KofCServiceRegistration
{
    public static IServiceCollection AddKofCService(this IServiceCollection services, IConfiguration config)
    {
        services.Configure<KofCApiOptions>(config.GetSection(KofCApiOptions.Key));

        var kofcApiOptions = services.BuildServiceProvider().GetRequiredService<IOptionsMonitor<KofCApiOptions>>().CurrentValue;
        services.AddKofC(new KofCApiConfig
        {
            BaseUrl = kofcApiOptions.BaseUrl
        });

        return services;
    }
}
