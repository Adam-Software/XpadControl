using System.Text.Json.Serialization;

namespace XpadControl.Interfaces.WebSocketClientsService.Dependencies.JsonModel
{
    public class ServoCommandsItem
    {
        [JsonPropertyName("name")]
        public ServoNames Name { get; set; }

        [JsonPropertyName("goal_position")]
        public int GoalPosition { get; set; }

    }
}
