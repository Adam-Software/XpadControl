using System.Text.Json.Serialization;


namespace XpadControl.Interfaces.Tests.WebSocketClientService.JsonModel
{
    public class Joint
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }
    }
}
