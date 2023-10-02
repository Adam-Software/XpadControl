using System;
using XInputium.XInput;
using XpadControl.Interfaces;
using XpadControl.Interfaces.GamepadService.Dependencies;
using XpadControl.Interfaces.GamepadService.Dependencies.EventArgs;
using XpadControl.Interfaces.GamepadService.Dependencies.EventArgs.PropertyChangedArgs;
using XpadControl.Windows.Services.Extensions;

namespace XpadControl.Windows.Services.GamepadService
{
    public class GamepadService : IGamepadService
    {
        #region Event axis/button/trigger

        public event LeftTriggerChangedEventHandler RaiseLeftTriggerChangedEvent;
        public event RightTriggerChangedEventHandler RaiseRightTriggerChangedEvent;
        public event ButtonChangedEventHandler RaiseButtonChangedEvent;
        public event LeftAxisChangedEventHandler RaiseLeftAxisChangedEvent;
        public event RightAxisChangedEventHandler RaiseRightAxisChangedEvent;

        #endregion

        #region Event connect/disconect

        public event ConnectedChangedEventHandler RaiseConnectedChangedEvent;

        #endregion

        private readonly XGamepad mGamepad;
        private readonly ILoggerService mLoggerService;

        public GamepadService(ILoggerService loggerService)
        {
            mLoggerService = loggerService;

            mLoggerService.WriteVerboseLog("OS is Windows");

            try
            {
                mGamepad = new XGamepad();
            }
            catch (Exception ex)
            {
                mLoggerService.WriteErrorLog($"Error when bind gamepad {ex.Message}");
            }

            if (mGamepad != null)
            {
                mGamepad.LeftJoystick.PropertyChanged += LeftJoystickPropertyChanged;
                mGamepad.RightJoystick.PropertyChanged += RightJoystickPropertyChanged;

                mGamepad.LeftTrigger.ValueChanged += LeftTriggerValueChanged;
                mGamepad.RightTrigger.ValueChanged += RightTriggerValueChanged;

                mGamepad.ButtonStateChanged += ButtonStateChanged;
                mGamepad.IsConnectedChanged += IsConnectedChanged;
            }
        }

        private void IsConnectedChanged(object sender, EventArgs e)
        {
            bool isConnected = mGamepad.IsConnected;
            OnRaiseConnectedChangedEvent(isConnected);

            mLoggerService.WriteInformationLog($"Gamepad conected state change. Now is {isConnected}");
        }

        public void Update() 
        {
            mGamepad.Update();
        }

        #region Gamepad event

        #region Buttons

        private void ButtonStateChanged(object sender, XInputium.DigitalButtonEventArgs<XInputButton> e)
        {
            OnRaiseButtonChangedEvent(e.Button.Button.ToButtons(), e.Button.IsPressed);
        }

        #endregion

        #region Axis

        private void LeftJoystickPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "X":
                    OnRaiseLeftAxisChangedEvent(mGamepad.LeftJoystick.X, mGamepad.LeftJoystick.Y, AxisPropertyChanged.X);
                    break;
                case "Y":
                    OnRaiseLeftAxisChangedEvent(mGamepad.LeftJoystick.X, mGamepad.LeftJoystick.Y, AxisPropertyChanged.Y);
                    break;
            }

            mLoggerService.WriteVerboseLog($"X {mGamepad.LeftJoystick.X} Y {mGamepad.LeftJoystick.Y}");
        }

        private void RightJoystickPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "X":
                    OnRaiseLeftAxisChangedEvent(mGamepad.RightJoystick.X, mGamepad.RightJoystick.Y, AxisPropertyChanged.X);
                    break;
                case "Y":
                    OnRaiseLeftAxisChangedEvent(mGamepad.RightJoystick.X, mGamepad.RightJoystick.Y, AxisPropertyChanged.Y);
                    break;

            }

            mLoggerService.WriteVerboseLog($"X {mGamepad.RightJoystick.X} Y {mGamepad.RightJoystick.Y}");
        }

        #endregion

        #region Trigger

        private void RightTriggerValueChanged(object sender, EventArgs e)
        {
            OnRaiseRightTriggerChangedEvent(mGamepad.RightTrigger.Value);

            mLoggerService.WriteVerboseLog($"{mGamepad.RightTrigger.Value}");
        }

        private void LeftTriggerValueChanged(object sender, EventArgs e)
        {
            OnRaiseLeftTriggerChangedEvent(mGamepad.LeftTrigger.Value);

            mLoggerService.WriteVerboseLog($"{mGamepad.LeftTrigger.Value}");
        }

        #endregion

        #endregion

        #region Raise events

        protected virtual void OnRaiseLeftAxisChangedEvent(float lx, float ly, AxisPropertyChanged axisChanged)
        {
            LeftAxisChangedEventHandler raiseEvent = RaiseLeftAxisChangedEvent;

            AxisEventArgs leftEventArgs = new()
            {
                X = lx,
                Y = ly
            };

            raiseEvent?.Invoke(this, axisChanged, leftEventArgs);
        }

        protected virtual void OnRaiseRightAxisChangedEvent(float rx, float ry, AxisPropertyChanged axisChanged)
        {
            RightAxisChangedEventHandler raiseEvent = RaiseRightAxisChangedEvent;

            AxisEventArgs rightEventArgs = new()
            {
                X = rx,
                Y = ry
            };

            raiseEvent?.Invoke(this, axisChanged, rightEventArgs);
        }

        protected virtual void OnRaiseLeftTriggerChangedEvent(float value)
        {
            LeftTriggerChangedEventHandler raiseEvent = RaiseLeftTriggerChangedEvent;

            TriggerEventArgs eventArgs = new()
            {
                Value = value
            };

            raiseEvent?.Invoke(this, eventArgs);
        }

        protected virtual void OnRaiseRightTriggerChangedEvent(float value)
        {
            RightTriggerChangedEventHandler raiseEvent = RaiseRightTriggerChangedEvent;

            TriggerEventArgs eventArgs = new()
            {
                Value = value
            };

            raiseEvent?.Invoke(this, eventArgs);
        }

        protected virtual void OnRaiseButtonChangedEvent(Buttons button, bool pressed)
        {
            ButtonChangedEventHandler raiseEvent = RaiseButtonChangedEvent;

            ButtonEventArgs eventArgs = new()
            {
                 Button = button,
                 Pressed = pressed
            };

            raiseEvent?.Invoke(this, eventArgs);
        }

        protected virtual void OnRaiseConnectedChangedEvent(bool isConnected)
        {
            ConnectedChangedEventHandler raiseEvent = RaiseConnectedChangedEvent;

            ConnectedEventArgs eventArgs = new()
            {
                IsConnected = isConnected
            };

            raiseEvent?.Invoke(this, eventArgs);
        }

        #endregion

        public void Dispose()
        {
            mLoggerService.WriteVerboseLog($"Dispose {nameof(GamepadService)} called");
        }

    }
}
