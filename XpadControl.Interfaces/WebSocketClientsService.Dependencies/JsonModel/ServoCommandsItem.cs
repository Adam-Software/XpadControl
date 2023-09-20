using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace XpadControl.Interfaces.WebSocketClientsService.Dependencies.JsonModel
{
    public class ServoCommandsItem
    {
        [JsonPropertyName("motors")]
        public List<ServoCommands> Motors { get; set; }
    }
}
