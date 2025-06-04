using KofCSDK;
using KofCSDK.Models.Requests;
using Microsoft.AspNetCore.Mvc;

namespace KofCApiProxy.ApiProxy;

public static class ApiProxyExtensionsNugetActivities
{
    public static void MapApiNugetActivities(this WebApplication app)
    {
        app.MapGet("api/{tenantId}/{version}/activities", async (
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

            var result = await kofcV1Client.GetAllActivities(
                tenantInfo,
                new UserAuthentication
                {
                    WebToken = token
                },
                cancellationToken);

            if (result.Success)
            {
                return Results.Ok(result.Data);
            }

            app.Logger.LogInformation("Error encountered getting all activities");

            return Results.Ok(result.Error);
        })
            .Produces<List<KofCSDK.Models.Responses.Activity>>(StatusCodes.Status200OK)
            .Produces<KofCSDK.Models.ProblemDetails>(StatusCodes.Status500InternalServerError);

        app.MapPost("api/{tenantId}/{version}/activities", async (
            HttpContext context,
            [FromRoute] string tenantId,
            [FromBody] CreateActivityRequest request,
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

            var result = await kofcV1Client.CreateActivity(
                tenantInfo,
                new AuthenticatedRequest<CreateActivityRequest>
                {
                    Payload = request,
                    UserAuthentication = new UserAuthentication
                    {
                        WebToken = token
                    }
                },
                cancellationToken);

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
