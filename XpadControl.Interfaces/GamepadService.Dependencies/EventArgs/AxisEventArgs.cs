using System.Runtime.CompilerServices;
using XpadControl.Interfaces.GamepadService.Dependencies.EventArgs.PropertyChangedArgs;

namespace XpadControl.Interfaces.GamepadService.Dependencies.EventArgs
{
    public class AxisEventArgs //: IAxisPropertyChanged
    {
        public float X { get; set; }
        public float Y { get; set; }
        //public event AxisPropertyChangedEventHandler PropertyChanged;

        //private float x;
        /*public float X
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
            AxisPropertyChangedEventHandler raiseEvent = PropertyChanged;
            AxisPropertyChangedEventArgs eventArgs = new AxisPropertyChangedEventArgs(propertyName, value);
            raiseEvent?.Invoke(this, eventArgs);
        }*/
    }
}
