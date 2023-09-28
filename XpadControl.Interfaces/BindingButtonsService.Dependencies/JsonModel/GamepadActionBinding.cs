using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace XpadControl.Interfaces.BindingButtonsService.Dependencies.JsonModel
{
    public class GamepadActionBinding
    {
        [JsonPropertyName("action_buttons")]
        public List<ButtonActionBinding> ButtonsAction { get; set; }

        [JsonPropertyName("sticks")]
        public List<SticksActionBinding> SticksAction { get; set; }

        [JsonPropertyName("triggers")]
        public List<TriggerActionBinding> TriggerAction { get; set; }
    }
}
