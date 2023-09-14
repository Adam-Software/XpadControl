using XpadControl.Interfaces.GamepadService.Dependencies;

namespace XpadControl.Linux.Services.Extensions
{
    public static class ButtonsExtension
    {
        public static Buttons ToButtons(this byte button)
        {
            int inputButtons = button;
            Buttons convertedButtons = (Buttons)inputButtons;

            return convertedButtons;
        }
    }
}
