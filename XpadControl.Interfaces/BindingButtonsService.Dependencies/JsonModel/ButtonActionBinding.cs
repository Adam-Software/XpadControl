﻿using System.Text.Json.Serialization;

namespace XpadControl.Interfaces.BindingButtonsService.Dependencies.JsonModel
{
    public class ButtonActionBinding
    {
        [JsonPropertyName("action")]
        public AdamActions Action { get; set; }

        [JsonPropertyName("button")]
        public ConfigButtons Button { get; set; }
    }
}
