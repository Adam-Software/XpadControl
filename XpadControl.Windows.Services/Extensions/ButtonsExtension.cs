using XInputium.XInput;
using XpadControl.Interfaces.GamepadService.Dependencies;

namespace XpadControl.Windows.Services.Extensions
{
    public static class ButtonsExtension
    {
        public static Buttons ToButtons(this XButtons xButtons)
        {
            int inputButtons = (int) xButtons;
            Buttons convertedButtons = (Buttons) inputButtons;

            return convertedButtons;            
        }
    }
}
