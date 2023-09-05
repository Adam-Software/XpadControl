using System;
using XInputium.XInput;
using XpadControl.Interfaces.GamepadService;
using XpadControl.Interfaces.GamepadService.Dependencies.EventArgs;
using XpadControl.Interfaces.LoggerService;

namespace XpadControl.Windows.Services.GamepadService
{
    public class GamepadService : IGamepadService
    {
        public event LeftAxisChangedEventHandler RaiseLeftAxisChangedEvent;
        public event RightAxisChangedEventHandler RaiseRightAxisChangedEvent;
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
                mGamepad.RightJoystickMove += MGamepad_RightJoystickMove;
                mGamepad.LeftJoystickMove += MGamepad_LeftJoystickMove;
                mGamepad.LeftJoystick.PositionChanged += LeftJoystickPositionChanged;
                mGamepad.RightJoystick.PositionChanged += RightJoystickPositionChanged;
            }
        }



        public void Update() 
        {
            mGamepad.Update();
        }

        #region Gamepad event

        private void MGamepad_LeftJoystickMove(object sender, EventArgs e)
        {
            OnRaiseLeftAxisChangedEvent(mGamepad.LeftJoystick.X, mGamepad.LeftJoystick.Y);
        }

        private void MGamepad_RightJoystickMove(object sender, EventArgs e)
        {
            OnRaiseRightAxisChangedEvent(mGamepad.RightJoystick.X, mGamepad.RightJoystick.Y);
        }

        private void LeftJoystickPositionChanged(object sender, EventArgs e)
        {
            mLoggerService.WriteVerboseLog($"X {mGamepad.LeftJoystick.X} Y {mGamepad.LeftJoystick.Y}");

            
        }

        private void RightJoystickPositionChanged(object sender, EventArgs e)
        {
            mLoggerService.WriteVerboseLog($"X {mGamepad.RightJoystick.X} Y {mGamepad.RightJoystick.Y}");

            
        }

        #endregion

        public void Dispose()
        {
            mLoggerService.WriteVerboseLog($"Dispose {nameof(GamepadService)} called");
        }

        #region Raise events

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

        protected virtual void OnRaiseButtonChangedEvent(byte button, bool pressed)
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
