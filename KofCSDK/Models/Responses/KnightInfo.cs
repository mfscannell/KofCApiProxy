using System.Text.Json.Serialization;

namespace KofCSDK.Models.Responses;

public class KnightInfo
{
    public Guid Id { get; set; }
    public long MemberNumber { get; set; }
    public bool MailReturned { get; set; }
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public KnightDegree Degree { get; set; }
    public DateTime? FirstDegreeDate { get; set; }
    public DateTime? ReentryDate { get; set; }
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public KnightMemberType MemberType { get; set; }
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public KnightMemberClass MemberClass { get; set; }
}
