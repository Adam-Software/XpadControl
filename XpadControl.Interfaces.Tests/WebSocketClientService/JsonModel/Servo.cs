using System.Text.Json.Serialization;

namespace XpadControl.Interfaces.Tests.WebSocketClientService.JsonModel
{
    public class Servo
    {
        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [JsonPropertyName("joint")]
        public Joint? Joint { get; set; }
    }
}
