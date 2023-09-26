using System.IO;
using System.Text.Json;
using XpadControl.Interfaces.BindingButtonsService.Dependencies.JsonModel;

namespace XpadControl.Interfaces.BindingButtonsService.Dependencies
{
    public static class Extensions
    {
        /// <summary>
        /// Read file wiht zero position and deserealize to GamepadActionBinding model
        /// </summary>
        public static GamepadActionBinding ToGamepadAction(this string jsonFilePath)
        {
            var options = new JsonSerializerOptions
            {
                ReadCommentHandling = JsonCommentHandling.Skip
            };

            using StreamReader reader = new StreamReader(jsonFilePath);
            var json = reader.ReadToEnd();
            var commands = JsonSerializer.Deserialize<GamepadActionBinding>(json, options);

            return commands;
        }

        public static GamepadService.Dependencies.Buttons ToGamepadButton(this Buttons buttons) 
        {
            switch (buttons)
            {
                case Buttons.Back:
                    return GamepadService.Dependencies.Buttons.Back;

                default:
                    return GamepadService.Dependencies.Buttons.None;
            }
        }
    }
}
