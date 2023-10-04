using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace XpadControl.Interfaces.WebSocketClientsService.Dependencies.JsonModel
{
    public class ServoCommands
    {
        [JsonPropertyName("motors")]
        public IList<ServoCommandsItem> Motors { get; set; }
    }
}
