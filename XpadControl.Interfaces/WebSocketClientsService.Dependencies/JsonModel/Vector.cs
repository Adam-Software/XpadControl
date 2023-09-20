using System.Text.Json.Serialization;

namespace XpadControl.JsonModel
{
    public class Vector
    {
        [JsonPropertyName("move")]
        public VectorItem Move { get; set; }
    }
}
