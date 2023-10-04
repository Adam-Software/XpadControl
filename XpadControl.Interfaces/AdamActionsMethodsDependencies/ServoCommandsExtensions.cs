using System;
using System.IO;
using System.Text.Json;
using XpadControl.Interfaces.WebSocketClientsService.Dependencies.JsonModel;

namespace XpadControl.Interfaces.AdamActionsMethodsDependencies
{
    public static class ServoCommandsExtensions
    {
        /// <summary>
        /// Read file wiht zero position and deserealize to ServoCommands model
        /// </summary>
        public static ServoCommands ToServoCommands(this string jsonFilePath)
        {
            var options = new JsonSerializerOptions
            {
                ReadCommentHandling = JsonCommentHandling.Skip
            };

            using StreamReader reader = new(jsonFilePath);
            var json = reader.ReadToEnd();
            ServoCommands commands = JsonSerializer.Deserialize<ServoCommands>(json, options);

            return commands;
        }

        /// <summary>
        /// Converting float gamepad value -1 .. 1 to servo range int value 0 .. 100
        /// </summary>
        public static int ToServoRange(this float gamepadRangeValue, int zeroPosition)
        {
            float floatValue = gamepadRangeValue * 100;
            int intValue = (int)Math.Round(floatValue) + zeroPosition;

            if (intValue < 0)
                intValue = 0;
            if (intValue > 100)
                intValue = 100;

            return intValue;
        }
    }
}
