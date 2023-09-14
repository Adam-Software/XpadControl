using System;
using XpadControl.Interfaces.GamepadService.Dependencies;

namespace XpadControl.Linux.Services.Extensions
{
    public static class ButtonsExtension
    {
        public static Buttons ToButtons(this byte button)
        {
            Console.WriteLine(button);
            int inputButtons = button;

            return inputButtons switch
            {
                0 => Buttons.A,
                1 => Buttons.B,
                2 => Buttons.X,
                3 => Buttons.Y,
                4 => Buttons.LB,
                5 => Buttons.RB,
                6 => Buttons.Back,
                7 => Buttons.Start,

                _ => Buttons.None,
            };
        }
    }
}
