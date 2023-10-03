using XpadControl.Interfaces.GamepadService.Dependencies.EventArgs.PropertyChangedArgs;

namespace XpadControl.Interfaces.GamepadService.Dependencies.EventArgs
{
    public class AxisEventArgs
    {
        public float X { get; set; }
        public float Y { get; set; }
        public AxisPropertyChanged AxisPropertyChanged { get; set; } 
    }
}
