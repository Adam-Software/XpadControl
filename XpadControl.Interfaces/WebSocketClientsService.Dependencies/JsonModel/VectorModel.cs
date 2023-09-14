using System.Text.Json.Serialization;

namespace XpadControl.JsonModel
{
    public class VectorModel
    {
        [JsonPropertyName("move")]
        public VectorItem Move { get; set; }
    }
}
