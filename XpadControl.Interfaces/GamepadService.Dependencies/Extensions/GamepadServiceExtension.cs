using System;

namespace XpadControl.Interfaces.GamepadService.Dependencies.Extensions
{
    public static class GamepadServiceExtension
    {
        /// <summary>
        /// Copy from <see cref="https://github.com/AderitoSilva/XInputium/blob/main/source/XInputium/XInputium/XInput/XInputDevice.cs"/>
        /// </summary>
        public static float ThumbToFloat(this short axis)
        {
            float floatAxis = ((float)axis) / (axis >= 0 ? 32767 : 32768);
            return Clamp11(floatAxis);
        }

        public static float TriggerToFloat(this short trigger)
        {
            //need calibrate or deadzone
            if (trigger == -32767)
                trigger = -32768;

            ushort usingedTrigger = (ushort) (trigger + 32768);
            var floatTrigger = ((float)usingedTrigger) / 65535;
            return Clamp11(floatTrigger);
        }

        /// <summary>
        /// Copy from <see cref="https://github.com/AderitoSilva/XInputium/blob/main/source/XInputium/XInputium/InputMath.cs
        /// 
        /// Truncates the specified <see cref="float"/> value to the -1 to 1 
        /// inclusive range.
        /// </summary>
        /// <param name="value">A <see cref="float"/> value to truncate.</param>
        /// <returns>-1 if <paramref name="value"/> is lower than or equal to -1, 
        /// 1 if <paramref name="value"/> is greater than or equal to 1, 
        /// or <paramref name="value"/>.</returns>
        /// <exception cref="ArgumentException">
        /// <paramref name="value"/> is <see cref="float.NaN"/>.</exception>
        public static float Clamp11(float value)
        {
            if (float.IsNaN(value))
                throw new ArgumentException(
                    $"'{float.NaN}' is not a valid value for '{nameof(value)}' parameter.",
                    nameof(value));

            if (value < -1f)
            {
                return (float)-1f;
            }
           
            if (value > 1f)
            {
                return (float)1f;
            }
                
            return (float)value;
        }
    }
}
