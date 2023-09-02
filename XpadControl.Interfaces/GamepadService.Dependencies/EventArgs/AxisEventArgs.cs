namespace XpadControl.Interfaces.GamepadService.Dependencies.EventArgs
{
    public class AxisEventArgs : System.EventArgs
    {
        public byte  Axis { get; set; }
        public short Value { get; set; }
    }
}
