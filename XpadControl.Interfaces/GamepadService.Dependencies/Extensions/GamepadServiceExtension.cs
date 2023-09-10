namespace XpadControl.Interfaces.GamepadService.Dependencies.Extensions
{
    public static class GamepadServiceExtension
    {
        public static float ToFloat(this short axis)
        {
            return ((float)axis) / (axis >= 0 ? 32767 : 32768);
        }
    }
}
