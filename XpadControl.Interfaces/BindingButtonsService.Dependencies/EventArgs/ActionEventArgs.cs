namespace XpadControl.Interfaces.BindingButtonsService.Dependencies.EventArgs
{
    public class ActionEventArgs : System.EventArgs
    {
        public AdamActions AdamActions { get; set; }
        public IsTrigger IsTrigger { get; set; } = IsTrigger.None;
        public IsAxis IsAxis { get; set; } = IsAxis.None;
        public IsButton IsButton { get; set; } = IsButton.None;

        /// <summary>
        /// From trriger and axis returned FloatValue
        /// Axis min value '-1'. Axis max value '1'
        /// Trriger min value 0. Trriger min value 1.
        /// </summary>
        public float FloatValue { get; set; }
    }
}
