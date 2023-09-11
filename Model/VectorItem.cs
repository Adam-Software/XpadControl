using System.Text.Json.Serialization;

namespace XpadControl.Model
{
    internal class VectorItem
    {
        [JsonPropertyName("x")]
        internal float X { get; set; }

        [JsonPropertyName("y")]
        internal float Y { get; set; }

        [JsonPropertyName("z")]
        internal float Z { get; set; }
    }
}
