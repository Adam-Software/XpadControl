namespace XpadControl.Interfaces.BindingButtonsService.Dependencies.EventArgs
{
    public class ActionEventArgs : System.EventArgs
    {
        public AdamActions AamActions { get; set; }
        public bool IsTrigger { get; set; }
        public bool IsAxis { get; set; }
        public bool IsButton { get; set; }
        public float FloatValue { get; set; }
        public bool ButtonValue { get; set; }
    }
}
