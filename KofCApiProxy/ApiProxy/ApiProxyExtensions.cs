using System.Diagnostics;
using System.Net;
using Yarp.ReverseProxy.Forwarder;

namespace KofCApiProxy.ApiProxy;

public static class ApiProxyExtensions
{
    public static IServiceCollection AddApiProxy(this IServiceCollection services)
    {
        services.AddHttpForwarder();

        return services;
    }

    public static void MapApiProxy(this WebApplication app)
    {
        var httpClient = new HttpMessageInvoker(new SocketsHttpHandler
        {
            UseProxy = false,
            AllowAutoRedirect = false,
            AutomaticDecompression = DecompressionMethods.None,
            UseCookies = false,
            EnableMultipleHttp2Connections = true,
            ActivityHeadersPropagator = new ReverseProxyPropagator(DistributedContextPropagator.Current),
            ConnectTimeout = TimeSpan.FromSeconds(15),
        });
        var requestConfig = new ForwarderRequestConfig { ActivityTimeout = TimeSpan.FromSeconds(100) };
        var transformer = HttpTransformer.Default; // or HttpTransformer.Default;

        var baseUrl = app.Configuration.GetValue<string>("KofCApi:BaseUrl");

        app.Map("api/{**catch-all}", async (HttpContext httpContext, IHttpForwarder forwarder) =>
        {
            var error = await forwarder.SendAsync(httpContext, baseUrl,
                httpClient, requestConfig, transformer);
            // Check if the operation was successful
            if (error != ForwarderError.None)
            {
                var errorFeature = httpContext.GetForwarderErrorFeature();
                var exception = errorFeature.Exception;
            }
        });
    }
}
