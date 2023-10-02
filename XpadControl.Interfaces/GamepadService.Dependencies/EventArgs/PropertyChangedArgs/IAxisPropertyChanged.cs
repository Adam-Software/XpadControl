using System;

namespace XpadControl.Interfaces.GamepadService.Dependencies.EventArgs.PropertyChangedArgs
{
    [Obsolete]
    public delegate void AxisPropertyChangedEventHandler(object sender, AxisPropertyChangedEventArgs e);

    [Obsolete]
    public interface IAxisPropertyChanged 
    {
        [Obsolete]
        public event AxisPropertyChangedEventHandler PropertyChanged;
    }
}
