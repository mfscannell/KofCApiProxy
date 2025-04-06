using Microsoft.Extensions.DependencyInjection;

namespace KofCSDK;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddKofC(this IServiceCollection services, KofCApiConfig kofcApiConfig)
    {
        services.AddTransient<IKofCV1Client, KofCV1Client>();
        services.AddHttpClient<IKofCV1Client, KofCV1Client>((sp, httpClient) =>
        {
            httpClient.BaseAddress = new Uri(kofcApiConfig.BaseUrl, UriKind.Absolute);
        });

        return services;
    }
}
