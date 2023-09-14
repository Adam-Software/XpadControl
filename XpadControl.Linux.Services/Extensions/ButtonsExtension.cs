using XpadControl.Interfaces.GamepadService.Dependencies;

namespace XpadControl.Linux.Services.Extensions
{
    public static class ButtonsExtension
    {
        public static Buttons ToButtons(this byte button)
        {
            int inputButtons = button;

            return inputButtons switch
            {
                0 => Buttons.A,
                1 => Buttons.B,
                _ => Buttons.None,
            };

            //Buttons convertedButtons = (Buttons)inputButtons;

            //return convertedButtons;
        }
    }
}
