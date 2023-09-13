using System;
using XInputium.XInput;
using XpadControl.Interfaces.GamepadService;
using XpadControl.Interfaces.GamepadService.Dependencies;
using XpadControl.Interfaces.GamepadService.Dependencies.EventArgs;
using XpadControl.Interfaces.LoggerService;

namespace XpadControl.Windows.Services.GamepadService
{
    public class GamepadService : IGamepadService
    {
        public event AxisChangedEventHandler RaiseAxisChangedEvent;
        public event LeftAxisChangedEventHandler RaiseLeftAxisChangedEvent;
        public event RightAxisChangedEventHandler RaiseRightAxisChangedEvent;

        public event LeftTriggerChangedEventHandler RaiseLeftTriggerChangedEvent;
        public event RightTriggerChangedEventHandler RaiseRightTriggerChangedEvent;

        public event ButtonChangedEventHandler RaiseButtonChangedEvent;
        
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

                mGamepad.ButtonPressed += ButtonPressed;
                mGamepad.ButtonReleased += ButtonReleased;

                mGamepad.ButtonStateChanged += ButtonStateChanged;
            }
        }

        private void ButtonStateChanged(object sender, XInputium.DigitalButtonEventArgs<XInputButton> e)
        {
            OnRaiseButtonChangedEvent(e.Button.Button, e.Button.IsPressed);
        }

        public void Update() 
        {
            mGamepad.Update();
        }

        #region Gamepad event

        #region Buttons

        private void ButtonReleased(object sender, XInputium.DigitalButtonEventArgs<XInputButton> e)
        {
            
            mLoggerService.WriteVerboseLog($"{e.Button} is false");
        }

        private void ButtonPressed(object sender, XInputium.DigitalButtonEventArgs<XInputButton> e)
        {
            mLoggerService.WriteVerboseLog($"{e.Button} is true");
        }

        #endregion

        #region Axis

        private void LeftJoystickPositionChanged(object sender, EventArgs e)
        {
            OnRaiseLeftAxisChangedEvent(mGamepad.LeftJoystick.X, mGamepad.LeftJoystick.Y);
            OnRaiseAxisChangedEvent(mGamepad.LeftJoystick.X, mGamepad.LeftJoystick.Y, mGamepad.RightJoystick.X, mGamepad.RightJoystick.Y);

            mLoggerService.WriteVerboseLog($"X {mGamepad.LeftJoystick.X} Y {mGamepad.LeftJoystick.Y}");
        }

        private void RightJoystickPositionChanged(object sender, EventArgs e)
        {
            OnRaiseRightAxisChangedEvent(mGamepad.RightJoystick.X, mGamepad.RightJoystick.Y);
            OnRaiseAxisChangedEvent(mGamepad.LeftJoystick.X, mGamepad.LeftJoystick.Y, mGamepad.RightJoystick.X, mGamepad.RightJoystick.Y);

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

        public void Dispose()
        {
            mLoggerService.WriteVerboseLog($"Dispose {nameof(GamepadService)} called");
        }

        #region Raise events

        protected virtual void OnRaiseAxisChangedEvent(float lx, float ly, float rx, float ry)
        {
            AxisChangedEventHandler raiseEvent = RaiseAxisChangedEvent;

            AxisEventArgs leftEventArgs = new()
            {
                Axis = 0,
                Value = 0,
                X = lx,
                Y = ly
            };

            AxisEventArgs rightEventArgs = new()
            {
                Axis = 0,
                Value = 0,
                X = rx,
                Y = ry
            };

            raiseEvent?.Invoke(this, leftEventArgs, rightEventArgs);
        }

        protected virtual void OnRaiseLeftAxisChangedEvent(float x, float y)
        {
            LeftAxisChangedEventHandler raiseEvent = RaiseLeftAxisChangedEvent;

            AxisEventArgs eventArgs = new()
            {
                 Axis = 0,
                 Value = 0,
                 X = x,
                 Y = y
            };

            raiseEvent?.Invoke(this, eventArgs);
        }

        protected virtual void OnRaiseRightAxisChangedEvent(float x, float y)
        {
            RightAxisChangedEventHandler raiseEvent = RaiseRightAxisChangedEvent;

            AxisEventArgs eventArgs = new()
            {
                Axis = 0,
                Value = 0,
                X = x,
                Y = y
            };

            raiseEvent?.Invoke(this, eventArgs);
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

        #endregion

    }
}
