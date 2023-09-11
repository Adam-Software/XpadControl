using System.Text.Json.Serialization;

namespace XpadControl.Model
{
    internal class VectorModel
    {
        [JsonPropertyName("move")]
        internal VectorItem Move { get; set; }
    }
}
