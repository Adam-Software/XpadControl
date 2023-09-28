using System.Text.Json.Serialization;

namespace XpadControl.Interfaces.BindingButtonsService.Dependencies.JsonModel
{
    public class SticksActionBinding
    {
        [JsonPropertyName("action")]
        public AdamActions Action { get; set; }

        [JsonPropertyName("axis")]
        public Sticks Sticks { get; set; }
    }
}
