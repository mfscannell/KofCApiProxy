namespace KofCSDK.Models.Responses;

public class StreetAddress
{
    public Guid Id { get; set; }
    public string? AddressName { get; set; }
    public string? Address1 { get; set; }
    public string? Address2 { get; set; }
    public string? City { get; set; }
    public string? StateCode { get; set; }
    public string? PostalCode { get; set; }
    public string? CountryCode { get; set; }
}
