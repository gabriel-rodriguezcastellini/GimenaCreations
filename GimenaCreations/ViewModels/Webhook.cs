using System.Text.Json.Serialization;

namespace GimenaCreations.ViewModels;

public class Webhook
{
    public string Action { get; set; }

    [JsonPropertyName("api_version")]
    public string ApiVersion { get; set; }

    public Data Data { get; set; }

    [JsonPropertyName("date_created")]
    public string DateCreated { get; set; }

    public long Id { get; set; }

    [JsonPropertyName("live_mode")]
    public bool LiveMode { get; set; }

    public string Type { get; set; }

    [JsonPropertyName("user_id")]
    public string UserId { get; set; }
}

public class Data
{
    public string Id { get; set; }
}