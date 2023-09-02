namespace XpadControl.Interfaces.GamepadService.Dependencies.EventArgs
{
    public class ButtonEventArgs : System.EventArgs
    {
        public byte Button { get; set; }
        public bool Pressed { get; set; }
    }
}
