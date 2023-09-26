using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace XpadControl.Interfaces.BindingButtonsService.Dependencies.JsonModel
{
    public class GamepadActionBinding
    {
        [JsonPropertyName("action_buttons")]
        public List<ButtonActionBinding> ButtonBindings { get; set; }

        [JsonPropertyName("sticks")]
        public List<SticksToActionBindingModel> SticksActions { get; set; }

        [JsonPropertyName("triggers")]
        public List<TriggerToActionBindingModel> TriggerActions { get; set; }
    }
}
