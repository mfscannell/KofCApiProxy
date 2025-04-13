using KofCSDK;
using KofCSDK.Models.Requests;
using KofCSDK.Models.Responses;
using Microsoft.AspNetCore.Mvc;

namespace KofCApiProxy.ApiProxy;

public static class ApiProxyExtensionsNugetKnights
{
    public static void MapApiNugetKnights(this WebApplication app)
    {
        app.MapGet("nuget/api/{tenantId}/{version}/knights", async (
            HttpContext context,
            [FromRoute] string tenantId,
            IKofCV1Client kofcV1Client,
            CancellationToken cancellationToken) =>
        {
            var tenantInfo = new TenantInfo
            {
                TenantId = tenantId
            };
            var token = context.Request.Headers.Authorization.ToString();

            if (!string.IsNullOrWhiteSpace(token) && token.StartsWith("Bearer "))
            {
                token = token.Substring(7);
            }

            var result = await kofcV1Client.GetAllKnights(
                tenantInfo,
                new UserAuthentication
                {
                    WebToken = token,
                },
                cancellationToken);

            if (result.Success)
            {
                return Results.Ok(result.Data);
            }

            app.Logger.LogInformation("Error encountered");

            return Results.Ok(result.Error);
        })
            .Produces<List<Knight>>(StatusCodes.Status200OK)
            .Produces<KofCSDK.Models.ProblemDetails>(StatusCodes.Status500InternalServerError);
    }
}
