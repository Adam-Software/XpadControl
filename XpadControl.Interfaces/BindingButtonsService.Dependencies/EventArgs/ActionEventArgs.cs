namespace XpadControl.Interfaces.BindingButtonsService.Dependencies.EventArgs
{

    /// <summary>
    /// From trriger and axis returned FloatValue
    /// From d-pad and button returned ButtonValue
    /// </summary>
    public class ActionEventArgs : System.EventArgs
    {
        public AdamActions AdamActions { get; set; }
        public bool IsTrigger { get; set; }
        public bool IsAxis { get; set; }
        public bool IsButton { get; set; }
        public float FloatValue { get; set; }
        public bool ButtonValue { get; set; }
    }
}
