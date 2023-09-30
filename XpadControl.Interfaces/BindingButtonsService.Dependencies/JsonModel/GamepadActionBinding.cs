using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace XpadControl.Interfaces.BindingButtonsService.Dependencies.JsonModel
{
    public class GamepadActionBinding
    {
        [JsonPropertyName("action_buttons")]
        public List<ButtonActionBinding> ButtonsAction { get; set; }

        [JsonPropertyName("option_buttons")]
        public List<ButtonActionBinding> ButtonsOption { get; set; }

        [JsonPropertyName("dpad")]
        public List<ButtonActionBinding> ButtonsDpad { get; set; }

        [JsonPropertyName("bumpers")]
        public List<ButtonActionBinding> ButtonsBumper { get; set; }

        [JsonPropertyName("sticks")]
        public List<SticksActionBinding> SticksAction { get; set; }

        [JsonPropertyName("sticks_buttons")]
        public List<ButtonActionBinding> SticksButtonsAction { get; set; }

        [JsonPropertyName("triggers")]
        public List<TriggerActionBinding> TriggerAction { get; set; }
    }
}
