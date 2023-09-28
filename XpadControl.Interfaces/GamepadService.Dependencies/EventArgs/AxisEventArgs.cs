using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace XpadControl.Interfaces.GamepadService.Dependencies.EventArgs
{
    public delegate void AxisPropertyChangedEventHandler(object sender, AxisPropertyChangedEventArgs e);

    public class AxisEventArgs : System.EventArgs
    {
        public event AxisPropertyChangedEventHandler RaiseAxisPropertyChangedEvent;

        private float x;
        public float X
        {
            get { return x; }
            set
            {
                x = value;

                NotifyPropertyChanged(X);
            }
        }

        private float y;
        public float Y
        {
            get { return y; }
            set
            {
                y = value;

                NotifyPropertyChanged(Y);
            }
        }

        protected virtual void NotifyPropertyChanged(float value, [CallerMemberName] string propertyName = "")
        {
            AxisPropertyChangedEventHandler raiseEvent = RaiseAxisPropertyChangedEvent;

            AxisPropertyChangedEventArgs eventArgs = new AxisPropertyChangedEventArgs(propertyName, value);

            RaiseAxisPropertyChangedEvent?.Invoke(this, eventArgs);
        }
    }

    public class AxisPropertyChangedEventArgs : PropertyChangedEventArgs
    {
        public AxisPropertyChangedEventArgs(string propertyName, float value) : base(propertyName)
        {
            Value = value;
        }

        public float Value { get; private set; }
    }
}
