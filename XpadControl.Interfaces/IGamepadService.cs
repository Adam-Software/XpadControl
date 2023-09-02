using System;
using XpadControl.Interfaces.GamepadService.Dependencies.EventArgs;

namespace XpadControl.Interfaces.GamepadService
{
    public delegate void AxisChangedEventHandler(object sender, AxisEventArgs e);
    public delegate void ButtonChangedEventHandler(object sender, ButtonEventArgs e);

    public interface IGamepadService : IDisposable
    {
        public event AxisChangedEventHandler RaiseAxisChangedEvent;
        public event ButtonChangedEventHandler RaiseButtonChangedEvent;
    }
}
