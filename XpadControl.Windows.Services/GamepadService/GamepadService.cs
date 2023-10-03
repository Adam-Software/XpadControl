using System;
using XInputium;
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
                mGamepad.LeftJoystick.PositionChanged += LeftJoystickPositionChanged;
                mGamepad.RightJoystick.PositionChanged += RightJoystickPositionChanged;

                mGamepad.LeftTrigger.ValueChanged += LeftTriggerValueChanged;
                mGamepad.RightTrigger.ValueChanged += RightTriggerValueChanged;

                mGamepad.ButtonStateChanged += ButtonStateChanged;
                mGamepad.IsConnectedChanged += IsConnectedChanged;
            }
        }

        public void Update()
        {
            mGamepad.Update();
        }

        #region Gamepad event

        #region IsConnected

        private void IsConnectedChanged(object sender, EventArgs e)
        {
            bool isConnected = mGamepad.IsConnected;
            OnRaiseConnectedChangedEvent(isConnected);

            mLoggerService.WriteInformationLog($"Gamepad conected state change. Now is {isConnected}");
        }

        #endregion

        #region Buttons

        private void ButtonStateChanged(object sender, DigitalButtonEventArgs<XInputButton> e)
        {
            OnRaiseButtonChangedEvent(e.Button.Button.ToButtons(), e.Button.IsPressed);
        }

        #endregion

        #region Axis

        private void LeftJoystickPositionChanged(object sender, EventArgs e)
        {
            LX = mGamepad.LeftJoystick.RawX;
            LY = mGamepad.LeftJoystick.RawY;
        }

        private void RightJoystickPositionChanged(object sender, EventArgs e)
        {
            RX = mGamepad.RightJoystick.RawX;
            RY = mGamepad.RightJoystick.RawY;
        }

        private float lx;
        private float LX 
        {
            get 
            { 
                return lx; 
            } 
            set 
            {
                if( lx == value )
                    return;

                lx = value;

                OnRaiseLeftAxisChangedEvent(LX, mGamepad.LeftJoystick.RawY, AxisPropertyChanged.X);
            }
        }

        private float ly;
        private float LY
        {
            get
            {
                return ly;
            }
            set
            {
                if (ly == value)
                    return;

                ly = value;

                OnRaiseLeftAxisChangedEvent(mGamepad.LeftJoystick.RawX, LY, AxisPropertyChanged.Y);
            }
        }

        private float rx;
        private float RX
        {
            get
            {
                return rx;
            }
            set
            {
                if (rx == value)
                    return;

                rx = value;

                OnRaiseRightAxisChangedEvent(RX, mGamepad.RightJoystick.RawY, AxisPropertyChanged.X);
            }
        }

        private float ry;
        private float RY
        {
            get
            {
                return ry;
            }
            set
            {
                if (ry == value)
                    return;

                ry = value;

                OnRaiseRightAxisChangedEvent(mGamepad.RightJoystick.RawX, RY, AxisPropertyChanged.Y);
            }
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

        protected virtual void OnRaiseLeftAxisChangedEvent(float lx, float ly, AxisPropertyChanged propertyChanged)
        {
            LeftAxisChangedEventHandler raiseEvent = RaiseLeftAxisChangedEvent;

            AxisEventArgs leftEventArgs = new()
            {
                X = lx,
                Y = ly,
                AxisPropertyChanged = propertyChanged
            };

            raiseEvent?.Invoke(this, leftEventArgs);
        }

        protected virtual void OnRaiseRightAxisChangedEvent(float rx, float ry, AxisPropertyChanged propertyChanged)
        {
            RightAxisChangedEventHandler raiseEvent = RaiseRightAxisChangedEvent;

            AxisEventArgs rightEventArgs = new()
            {
                X = rx,
                Y = ry,
                AxisPropertyChanged = propertyChanged
            };

            raiseEvent?.Invoke(this, rightEventArgs);
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
            mGamepad.LeftJoystick.PositionChanged -= LeftJoystickPositionChanged;
            mGamepad.RightJoystick.PositionChanged -= RightJoystickPositionChanged;

            mGamepad.LeftTrigger.ValueChanged -= LeftTriggerValueChanged;
            mGamepad.RightTrigger.ValueChanged -= RightTriggerValueChanged;

            mGamepad.ButtonStateChanged -= ButtonStateChanged;
            mGamepad.IsConnectedChanged -= IsConnectedChanged;

            mLoggerService.WriteVerboseLog($"Dispose {nameof(GamepadService)} called");
        }
    }
}
