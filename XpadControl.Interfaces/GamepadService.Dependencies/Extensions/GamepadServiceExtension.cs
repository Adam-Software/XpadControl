namespace XpadControl.Interfaces.GamepadService.Dependencies.Extensions
{
    public static class GamepadServiceExtension
    {
        /// <summary>
        /// Copy from <see cref="https://github.com/AderitoSilva/XInputium/blob/main/source/XInputium/XInputium/XInput/XInputDevice.cs"/>
        /// </summary>
        public static float ThumbToFloat(this short axis)
        {
            return ((float)axis) / (axis >= 0 ? 32767 : 32768);
        }

        /// <summary>
        /// Copy from <see cref="https://github.com/AderitoSilva/XInputium/blob/main/source/XInputium/XInputium/XInput/XInputDevice.cs"/>
        /// </summary>
        /*public static float TriggerToFloat(this byte axis) 
        {
            return ((float)axis) / byte.MaxValue;   
        }*/
    }
}
