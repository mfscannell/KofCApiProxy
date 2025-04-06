﻿using KofCSDK;
using KofCSDK.Models.Requests;
using KofCSDK.Models.Responses;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace KofCApiProxy.ApiProxy;

public static class ApiProxyExtensionsNugetAccounts
{
    public static void MapApiNugetAccounts(this WebApplication app)
    {
        app.MapPost("nuget/api/{tenantId}/{version}/accounts/login", async (
            [FromRoute] string tenantId,
            [FromBody] LoginRequest loginRequest,
            IKofCV1Client kofcV1Client,
            CancellationToken cancellationToken) =>
        {
            app.Logger.LogInformation("Login request:");
            var tenantInfo = new TenantInfo
            {
                TenantId = tenantId
            };
            var result = await kofcV1Client.LoginAsync(tenantInfo, loginRequest, cancellationToken);

            if (result.StatusCode == HttpStatusCode.OK)
            {
                return Results.Ok(result.Data);
            }

            return Results.Ok(result.Error);
        })
            .Produces<LoginResponse>(StatusCodes.Status200OK)
            .Produces<KofCSDK.Models.ProblemDetails>(StatusCodes.Status400BadRequest)
            .Produces<KofCSDK.Models.ProblemDetails>(StatusCodes.Status500InternalServerError);

        app.MapGet("nuget/api/{tenantId}/{version}/accounts/passwordRequirements", async (
            [FromRoute] string tenantId,
            [FromBody] UserAuthentication userAuthentication,
            IKofCV1Client kofcV1Client,
            CancellationToken cancellationToken) =>
        {
            var tenantInfo = new TenantInfo
            {
                TenantId = tenantId
            };
            var result = await kofcV1Client.GetPasswordRequirementsAsync(
                tenantInfo,
                userAuthentication,
                cancellationToken);

            if (result.StatusCode == HttpStatusCode.OK)
            {
                return Results.Ok(result.Data);
            }

            return Results.Ok(result.Error);
        })
            .Produces<LoginResponse>(StatusCodes.Status200OK)
            .Produces<KofCSDK.Models.ProblemDetails>(StatusCodes.Status400BadRequest)
            .Produces<KofCSDK.Models.ProblemDetails>(StatusCodes.Status500InternalServerError);
    }
}
