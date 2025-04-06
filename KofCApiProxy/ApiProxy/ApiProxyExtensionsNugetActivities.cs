using KofCSDK;
using KofCSDK.Models.Requests;
using Microsoft.AspNetCore.Mvc;

namespace KofCApiProxy.ApiProxy;

public static class ApiProxyExtensionsNugetActivities
{
    public static void MapApiNugetActivities(this WebApplication app)
    {
        app.MapGet("nuget/api/{tenantId}/{version}/activities", async (
            [FromRoute] string tenantId,
            [FromBody] UserAuthentication userAuthentication,
            IKofCV1Client kofcV1Client,
            CancellationToken cancellationToken) =>
        {
            var tenantInfo = new TenantInfo
            {
                TenantId = tenantId
            };
            var result = await kofcV1Client.GetAllActivities(
                tenantInfo,
                userAuthentication,
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

        app.MapPost("nuget/api/{tenantId}/{version}/activities", async (
            [FromRoute] string tenantId,
            [FromBody] AuthenticatedRequest<CreateActivityRequest> request,
            IKofCV1Client kofcV1Client,
            CancellationToken cancellationToken) =>
        {
            var tenantInfo = new TenantInfo
            {
                TenantId = tenantId
            };
            var result = await kofcV1Client.CreateActivity(
                tenantInfo,
                request,
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
