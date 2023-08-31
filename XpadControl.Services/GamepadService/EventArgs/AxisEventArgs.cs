namespace XpadControl.Services.GamepadService.EventArgs
{
    public class AxisEventArgs : System.EventArgs
    {
        public byte  Axis { get; set; }
        public short Value { get; set; }
    }
}
