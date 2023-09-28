namespace XpadControl.Interfaces.GamepadService.Dependencies.EventArgs.PropertyChangedArgs
{
    public class AxisPropertyChangedEventArgs : System.ComponentModel.PropertyChangedEventArgs
    {
        public AxisPropertyChangedEventArgs(string propertyName, float value) : base(propertyName)
        {
            Value = value;
        }

        public float Value { get; private set; }
    }
}
