using System;
using XpadControl.Interfaces.GamepadService.Dependencies;
using MyButtonEventArgs = XpadControl.Interfaces.GamepadService.Dependencies.EventArgs.ButtonEventArgs;

namespace XpadControl.Linux.Services.Extensions
{
    public static class ButtonsExtension
    {
        private static readonly Buttons dPadLeftButton = ((byte)0x9).ToButtons();
        private static readonly Buttons dPadRightButton = ((byte)0x10).ToButtons();
        private static readonly Buttons dPadDownButton = ((byte)0x11).ToButtons();
        private static readonly Buttons dPadUpButton = ((byte)0x12).ToButtons();

        private static bool dpadLeftPressed = false;
        private static bool dpadRightPressed = false;
        private static bool dpadUpPressed = false;
        private static bool dpadDownPressed = false;

        public static Buttons ToButtons(this byte button)
        {
            int inputButtons = button;

            return inputButtons switch
            {
                0x0 => Buttons.A,
                0x1 => Buttons.B,
                0x2 => Buttons.X,
                0x3 => Buttons.Y,
                0x4 => Buttons.LB,
                0x5 => Buttons.RB,
                0x6 => Buttons.Back,
                0x7 => Buttons.Start,
                0x9 => Buttons.DPadLeft,
                0x10 => Buttons.DPadRight,
                0x11 => Buttons.DPadDown,
                0x12 => Buttons.DPadUp,

                _ => Buttons.None,
            };
        }

        private static MyButtonEventArgs MyButtonEventArgs(Buttons button, bool pressed) 
            => new() { Button = button, Pressed = pressed};

        public static MyButtonEventArgs ToButtonEventArgs(this short value, bool isAxisX) 
        {
            short xpad = value;

            switch (xpad)
            {
                case > 0:

                    if (isAxisX)
                    {
                        dpadRightPressed = true;

                        return MyButtonEventArgs(dPadRightButton, dpadRightPressed);
                    }

                    if (!isAxisX)
                    {
                        dpadDownPressed = true;

                        return MyButtonEventArgs(dPadDownButton, dpadDownPressed);
                    }

                    break;

                case < 0:

                    if (isAxisX)
                    {
                        dpadLeftPressed = true;

                        return MyButtonEventArgs(dPadLeftButton, dpadLeftPressed);
                    }

                    if (!isAxisX)
                    {
                        dpadUpPressed = true;

                        return MyButtonEventArgs(dPadUpButton, dpadUpPressed);
                    }

                    break;

                case 0:

                    if (dpadRightPressed)
                    {
                        dpadRightPressed = false;

                        return MyButtonEventArgs(dPadRightButton, dpadRightPressed);
                    }

                    if (dpadLeftPressed)
                    {
                        dpadLeftPressed = false;

                        return MyButtonEventArgs(dPadLeftButton, dpadLeftPressed);
                    }

                    if (dpadUpPressed)
                    {
                        dpadUpPressed = false;

                        return MyButtonEventArgs(dPadUpButton, dpadUpPressed);
                    }

                    if (dpadDownPressed)
                    {
                        dpadDownPressed = false;

                        return MyButtonEventArgs(dPadDownButton, dpadDownPressed);
                    }

                    break;
            }
            return new MyButtonEventArgs();
        }
    }
}
