using KofCSDK;
using KofCSDK.Models.Requests;
using KofCSDK.Models.Responses;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
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

        app.Map("api/{**catch-all}", async (HttpContext httpContext, IHttpForwarder forwarder) =>
        {
            var error = await forwarder.SendAsync(httpContext, "http://localhost:9080/",
                httpClient, requestConfig, transformer);
            // Check if the operation was successful
            if (error != ForwarderError.None)
            {
                var errorFeature = httpContext.GetForwarderErrorFeature();
                var exception = errorFeature.Exception;
            }
        });
    }

    public static void MapApiNuget(this WebApplication app)
    {
        app.MapPost("nuget/api/{tenantId}/accounts/login", async (
            [FromRoute]string tenantId,
            [FromBody]LoginRequest loginRequest,
            IKofCV1Client  kofcV1Client) =>
        {
            var tenantInfo = new TenantInfo
            {
                TenantId = tenantId
            };
            var result = await kofcV1Client.LoginAsync(tenantInfo, loginRequest);

            if (result.StatusCode == HttpStatusCode.OK)
            {
                return Results.Ok(result.Data);
            }

            return Results.Ok(result.Error);
        })
            .Produces<LoginResponse>(StatusCodes.Status200OK)
            .Produces<KofCSDK.Models.ProblemDetails>(StatusCodes.Status400BadRequest)
            .Produces<KofCSDK.Models.ProblemDetails>(StatusCodes.Status500InternalServerError);

        app.MapGet("nuget/api/{tenantId}/activities", async (
            [FromRoute] string tenantId,
            [FromBody] UserAuthentication userAuthentication,
            IKofCV1Client kofcV1Client,
            CancellationToken cancellationToken) =>
        {
            //return 1;
            var tenantInfo = new TenantInfo
            {
                TenantId = tenantId
            };
            var result = await kofcV1Client.GetAllActivities(
                tenantInfo,
                userAuthentication);

            if (result.Success)
            {
                return Results.Ok(result.Data);
            }

            return Results.Ok(result.Error);
        })
            .Produces<KofCSDK.Models.Responses.Activity>(StatusCodes.Status200OK)
            .Produces<KofCSDK.Models.ProblemDetails>(StatusCodes.Status500InternalServerError);
    }
}
