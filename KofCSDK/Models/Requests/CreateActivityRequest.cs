using KofCSDK.Models.Responses;
using System.Text.Json.Serialization;

namespace KofCSDK.Models.Requests;

public class CreateActivityRequest
{
    public CreateActivityRequest()
    {
        ActivityCoordinators = new List<Guid>();
    }
    public string ActivityName { get; set; } = string.Empty;
    public string? ActivityDescription { get; set; }
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public ActivityCategory ActivityCategory { get; set; }
    public IList<Guid> ActivityCoordinators { get; set; }
    public string? Notes { get; set; }
}
