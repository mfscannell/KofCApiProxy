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