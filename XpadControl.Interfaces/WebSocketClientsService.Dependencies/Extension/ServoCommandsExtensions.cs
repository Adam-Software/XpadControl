using System.IO;
using System.Text.Json;
using XpadControl.Interfaces.WebSocketClientsService.Dependencies.JsonModel;

namespace XpadControl.Interfaces.WebSocketClientsService.Dependencies.Extension
{
    public static class ServoCommandsExtensions
    {
        public static ServoCommands ToServoCommands(this string jsonFilePath)
        {
            var options = new JsonSerializerOptions
            {
                ReadCommentHandling = JsonCommentHandling.Skip
            };

            using StreamReader reader = new StreamReader(jsonFilePath);
            var json = reader.ReadToEnd();
            ServoCommands commands = JsonSerializer.Deserialize<ServoCommands>(json, options);

            return commands;
        }
    }
}
