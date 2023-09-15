using System;
using XpadControl.Interfaces.GamepadService.Dependencies.EventArgs;

namespace XpadControl.Interfaces.GamepadService
{
    public delegate void AxisChangedEventHandler(object sender, AxisEventArgs left, AxisEventArgs right);
    
    public delegate void LeftTriggerChangedEventHandler(object sender, TriggerEventArgs e);
    public delegate void RightTriggerChangedEventHandler(object sender, TriggerEventArgs e);

    public delegate void ButtonChangedEventHandler(object sender, ButtonEventArgs e);

    public interface IGamepadService : IDisposable
    {
        public event AxisChangedEventHandler RaiseAxisChangedEvent;
    
        public event LeftTriggerChangedEventHandler RaiseLeftTriggerChangedEvent;
        public event RightTriggerChangedEventHandler RaiseRightTriggerChangedEvent;

        public event ButtonChangedEventHandler RaiseButtonChangedEvent;

        // Call this on every app/game frame if needed
        public virtual void Update() { }

        //protected virtual void OnRaiseAxisChangedEvent(byte axis, short value, float lx, float ly, float rx, float ry) { }
    }
}
