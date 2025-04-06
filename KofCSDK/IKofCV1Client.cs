using KofCSDK.Models;
using KofCSDK.Models.Requests;
using KofCSDK.Models.Responses;

namespace KofCSDK;

public interface IKofCV1Client
{
    Task<Result<LoginResponse>> LoginAsync(
        TenantInfo tenantInfo,
        LoginRequest request,
        CancellationToken cancellationToken = default);
    Task<Result<PasswordRequirements>> GetPasswordRequirementsAsync(
        TenantInfo tenantInfo,
        UserAuthentication userAuthentication,
        CancellationToken cancellationToken = default);
    Task<Result<List<Activity>>> GetAllActivities(
        TenantInfo tenantInfo,
        UserAuthentication userAuthentication,
        CancellationToken cancellationToken = default);
    Task<Result<Activity>> CreateActivity(
        TenantInfo tenantInfo,
        AuthenticatedRequest<CreateActivityRequest> request,
        CancellationToken cancellationToken = default);
    Task<Result<List<Knight>>> GetAllKnights(
        TenantInfo tenantInfo,
        UserAuthentication userAuthentication,
        CancellationToken cancellationToken = default);
}
