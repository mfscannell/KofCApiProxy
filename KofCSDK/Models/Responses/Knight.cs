namespace KofCSDK.Models.Responses;

public class Knight
{
    public Guid Id { get; set; }
    public string FirstName { get; set; }
    public string MiddleName { get; set; }
    public string LastName { get; set; }
    public string NameSuffix { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public string EmailAddress { get; set; }
    public string CellPhoneNumber { get; set; }
    public StreetAddress? HomeAddress { get; set; }
    public KnightInfo? KnightInfo { get; set; }
}
