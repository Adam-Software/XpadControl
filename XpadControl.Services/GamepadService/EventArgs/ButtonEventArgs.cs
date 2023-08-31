namespace XpadControl.Services.GamepadService.EventArgs
{
    public class ButtonEventArgs : System.EventArgs
    {
        public byte Button { get; set; }
        public bool Pressed { get; set; }
    }
}
