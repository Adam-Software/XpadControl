using System.IO;
using System.Text.Json;
using XpadControl.Interfaces.BindingButtonsService.Dependencies.EventArgs;
using XpadControl.Interfaces.BindingButtonsService.Dependencies.JsonModel;
using XpadControl.Interfaces.GamepadService.Dependencies;

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

        public static ConfigButtons ToConfigButton(this Buttons buttons) 
        {
            int value = (int) buttons;
            return (ConfigButtons)value;
        }

        public static IsButton ToButtonEventArgs(this bool isPressed)
        {
            switch (isPressed)
            {
                case true:
                    return IsButton.IsButtonPressed;
                case false:
                    return IsButton.IsButtonReleased;    
            }
        }
    }
}
