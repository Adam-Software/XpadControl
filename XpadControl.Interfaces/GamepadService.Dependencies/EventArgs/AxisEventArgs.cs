namespace XpadControl.Interfaces.GamepadService.Dependencies.EventArgs
{
    public class AxisEventArgs : System.EventArgs
    {
        public byte  Axis { get; set; }
        public short Value { get; set; }
        public float X { get; set; }
        public float Y { get; set; }
    }
}
