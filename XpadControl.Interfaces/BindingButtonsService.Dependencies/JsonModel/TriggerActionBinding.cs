
using System.Text.Json.Serialization;

namespace XpadControl.Interfaces.BindingButtonsService.Dependencies.JsonModel
{
    public class TriggerActionBinding
    {
        [JsonPropertyName("action")]
        public AdamActions Action { get; set; }

        [JsonPropertyName("trigger")]
        public Triggers Trigger { get; set; }
    }
}
