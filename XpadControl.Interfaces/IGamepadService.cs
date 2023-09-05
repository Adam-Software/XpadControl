using System;
using XpadControl.Interfaces.GamepadService.Dependencies.EventArgs;

namespace XpadControl.Interfaces.GamepadService
{
    public delegate void LeftAxisChangedEventHandler(object sender, AxisEventArgs e);
    public delegate void RightAxisChangedEventHandler(object sender, AxisEventArgs e);
    public delegate void ButtonChangedEventHandler(object sender, ButtonEventArgs e);

    public interface IGamepadService : IDisposable
    {
        public event LeftAxisChangedEventHandler RaiseLeftAxisChangedEvent;
        public event RightAxisChangedEventHandler RaiseRightAxisChangedEvent;
        public event ButtonChangedEventHandler RaiseButtonChangedEvent;

        // Call this on every app/game frame if needed
        public virtual void Update() { }
    }
}
