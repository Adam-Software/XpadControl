using System;
using XpadControl.Interfaces.GamepadService.Dependencies.EventArgs;
using XpadControl.Interfaces.GamepadService.Dependencies.EventArgs.PropertyChangedArgs;

namespace XpadControl.Interfaces
{
    #region Delegate axis/button/trigger

    public delegate void LeftAxisChangedEventHandler(object sender, AxisPropertyChanged axisChanged, AxisEventArgs eventArgs);
    public delegate void RightAxisChangedEventHandler(object sender, AxisPropertyChanged axisChanged, AxisEventArgs eventArgs);

    public delegate void LeftTriggerChangedEventHandler(object sender, TriggerEventArgs e);
    public delegate void RightTriggerChangedEventHandler(object sender, TriggerEventArgs e);

    public delegate void ButtonChangedEventHandler(object sender, ButtonEventArgs e);

    #endregion

    #region Delegate connect/disconect

    public delegate void ConnectedChangedEventHandler(object sender, ConnectedEventArgs e);

    #endregion

    public interface IGamepadService : IDisposable
    {
        #region Event axis/button/trigger

        public event LeftAxisChangedEventHandler RaiseLeftAxisChangedEvent;
        public event RightAxisChangedEventHandler RaiseRightAxisChangedEvent;
    
        public event LeftTriggerChangedEventHandler RaiseLeftTriggerChangedEvent;
        public event RightTriggerChangedEventHandler RaiseRightTriggerChangedEvent;

        public event ButtonChangedEventHandler RaiseButtonChangedEvent;

        #endregion

        #region Event connect/disconect

        public event ConnectedChangedEventHandler RaiseConnectedChangedEvent;

        #endregion

        // Call this on every app/game frame in windows
        // Call this for check connected/disconected in linux
        public void Update();

    }
}
