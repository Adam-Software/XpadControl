using System;
using System.Diagnostics;
using System.Threading.Tasks;
using XInputium.XInput;
using XpadControl.Interfaces.GamepadService;
using XpadControl.Interfaces.GamepadService.Dependencies.EventArgs;
using XpadControl.Interfaces.LoggerService;

namespace XpadControl.Windows.Services.GamepadService
{
    public class GamepadService :  IGamepadService
    {
        public event AxisChangedEventHandler RaiseAxisChangedEvent;
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
                mGamepad.LeftJoystick.PositionChanged += LeftJoystick_PositionChanged;
                mGamepad.ButtonPressed += MGamepad_ButtonPressed;
            }
            
            Task.Run(Start);
        }
        private Task Start()
        {
            try
            {
                while (true)
                {
                    mGamepad.Update();
                }
            }
            catch (Exception ex)
            {
                mLoggerService.WriteErrorLog($"{ex.Message}");
            }

            return Task.CompletedTask;
        }


        private void MGamepad_ButtonPressed(object sender, XInputium.DigitalButtonEventArgs<XInputButton> e)
        {
            mLoggerService.WriteVerboseLog($"{e.Button}");
        }

        private void LeftJoystick_PositionChanged(object sender, EventArgs e)
        {
            mLoggerService.WriteVerboseLog($"X {mGamepad.LeftJoystick.RawX} Y {mGamepad.LeftJoystick.RawY}");
        }

        public void Dispose()
        {
            mLoggerService.WriteVerboseLog($"Dispose {nameof(GamepadService)} called");
        }

        #region Linux gamepad event

        //private void ButtonChanged(object sender, Gamepad.ButtonEventArgs e)
        //{
        //    mLoggerService.WriteVerboseLog($"{e.Button} is {e.Pressed}");
        //
        //   OnRaiseButtonChangedEvent(e.Button, e.Pressed);
        //}

        //private void AxisChanged(object sender, Gamepad.AxisEventArgs e)
        //{
        //    mLoggerService.WriteVerboseLog($"{e.Axis} is {e.Value}");

        //    OnRaiseAxisChangedEvent(e.Axis, e.Value);
        //}

        #endregion

        #region Raise events

        protected virtual void OnRaiseAxisChangedEvent(byte axis, short value)
        {
            AxisChangedEventHandler raiseEvent = RaiseAxisChangedEvent;

            AxisEventArgs eventArgs = new()
            {
                 Axis = axis,
                 Value = value
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
