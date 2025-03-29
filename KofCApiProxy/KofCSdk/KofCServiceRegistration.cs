using KofCSDK;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace KofCApiProxy.Options;

public static class KofCServiceRegistration
{
    public static IServiceCollection AddKofCService(this IServiceCollection services, IConfiguration config)
    {
        services.Configure<KofCApiOptions>(config.GetSection(KofCApiOptions.Key));

        //services.AddHttpClient(KofCApiOptions.HttpClientName, (serviceProvider, httpClient) =>
        //{
        //    var kofcClientConfig = serviceProvider.GetRequiredService<IOptionsMonitor<KofCApiOptions>>().CurrentValue;
        //    httpClient.BaseAddress = new Uri(kofcClientConfig.BaseUrl);
        //});
        var kofcApiOptions = services.BuildServiceProvider().GetRequiredService<IOptionsMonitor<KofCApiOptions>>().CurrentValue;
        services.AddKofC(new KofCApiConfig
        {
            HttpClientName = KofCApiOptions.HttpClientName,
            BaseUrl = kofcApiOptions.BaseUrl
        });

        return services;
    }
}
