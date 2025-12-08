namespace KofCSDK.Models.Requests;

public class AuthenticatedRequest
{
    public UserAuthentication UserAuthentication { get; set; }
}

public class AuthenticatedRequest<T>
{
    public T Payload { get; set; }
    public UserAuthentication UserAuthentication { get; set; }
}

public interface IHasTenantInfo
{
    TenantInfo TenantInfo { get; set; }
}

public class AuthenticatedTenantRequest : AuthenticatedRequest, IHasTenantInfo
{
    public TenantInfo TenantInfo { get; set; }
}

public class AuthenticatedTenantRequest<T> : AuthenticatedRequest<T>, IHasTenantInfo
{
    public TenantInfo TenantInfo { get; set; }
}