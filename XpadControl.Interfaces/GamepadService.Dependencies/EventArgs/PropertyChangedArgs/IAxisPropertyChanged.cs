namespace XpadControl.Interfaces.GamepadService.Dependencies.EventArgs.PropertyChangedArgs
{
    public delegate void AxisPropertyChangedEventHandler(object sender, AxisPropertyChangedEventArgs e);

    public interface IAxisPropertyChanged 
    {
        public event AxisPropertyChangedEventHandler PropertyChanged;
    }
}
