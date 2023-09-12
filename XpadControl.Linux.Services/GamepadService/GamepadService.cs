using Gamepad;
using System;
using XpadControl.Interfaces.GamepadService;
using XpadControl.Interfaces.LoggerService;
using XpadControl.Interfaces.GamepadService.Dependencies.Extensions;
using MyAxisEventArgs = XpadControl.Interfaces.GamepadService.Dependencies.EventArgs.AxisEventArgs;
using MyButtonEventArgs = XpadControl.Interfaces.GamepadService.Dependencies.EventArgs.ButtonEventArgs;
using MyTriggerEventArgs = XpadControl.Interfaces.GamepadService.Dependencies.EventArgs.TriggerEventArgs;

namespace XpadControl.Linux.Services.GamepadService
{
    public class GamepadService : IGamepadService
    {
        public event AxisChangedEventHandler RaiseAxisChangedEvent;
        public event LeftAxisChangedEventHandler RaiseLeftAxisChangedEvent;
        public event RightAxisChangedEventHandler RaiseRightAxisChangedEvent;

        public event LeftTriggerChangedEventHandler RaiseLeftTriggerChangedEvent;
        public event RightTriggerChangedEventHandler RaiseRightTriggerChangedEvent;

        public event ButtonChangedEventHandler RaiseButtonChangedEvent;

        private readonly GamepadController mGamepad;
        private readonly ILoggerService mLoggerService;

        public GamepadService(ILoggerService loggerService) 
        {
            mLoggerService = loggerService;

            mLoggerService.WriteVerboseLog("OS is Linux");
           
            try
            {
                mGamepad = new GamepadController("/dev/input/js0");    
            }
            catch (Exception ex)
            {
                mLoggerService.WriteErrorLog($"Error when bind gamepad {ex.Message}");
            }
            
            if (mGamepad != null)
            {
                mGamepad.AxisChanged += AxisChanged;
                mGamepad.ButtonChanged += ButtonChanged;
            }    
        }

        public void Dispose()
        {
            mLoggerService.WriteVerboseLog($"Dispose {nameof(GamepadService)} called");

            mGamepad?.Dispose();
        }


        #region Gamepad event

        private void ButtonChanged(object sender, ButtonEventArgs e)
        {
            mLoggerService.WriteVerboseLog($"{e.Button} is {e.Pressed}");

            OnRaiseButtonChangedEvent(e.Button, e.Pressed);
        }

        float lx = 0, ly = 0, rx = 0, ry = 0;

        private void AxisChanged(object sender, AxisEventArgs e)
        {
            byte axis = e.Axis;
            short value = e.Value;

            mLoggerService.WriteVerboseLog($"{axis} is {value}");

            switch (axis)
            {
                case 0:
                    lx = value.ThumbToFloat();
                    mLoggerService.WriteVerboseLog($"LEFT STICK X:{lx} or {value}");
                    break;
                case 2:
                    OnRaiseLeftTriggerChangedEvent(value.ThumbToFloat());
                    mLoggerService.WriteVerboseLog($"LEFT TRIGGER value is {value}");
                    break;
                case 1:
                    ly = -value.ThumbToFloat();
                    mLoggerService.WriteVerboseLog($"LEFT STICK Y:{ly} or {value}");
                    break; 
                case 3:
                    rx = value.ThumbToFloat();
                    mLoggerService.WriteVerboseLog($"RIGHT STICK X:{rx} or {value}");
                    break;
                case 4:
                    ry = -value.ThumbToFloat();
                    mLoggerService.WriteVerboseLog($"RIGHT STICK Y:{ry} or {value}");
                    break;
                case 5:
                    OnRaiseRightTriggerChangedEvent(value.ThumbToFloat());
                    mLoggerService.WriteVerboseLog($"RIGHT TRIGGER value is {value}");
                    break;
            }

            OnRaiseAxisChangedEvent(axis, value, lx, ly, rx, ry);

            switch (axis)
            {
                case <1:
                    OnRaiseLeftAxisChangedEvent(axis, value, lx, ly);
                    break;

                case >=3 and <=4:
                    OnRaiseRightAxisChangedEvent(axis, value, rx, ry);
                    break;
            }
        }

        #endregion

        #region Raise events

        protected virtual void OnRaiseAxisChangedEvent(byte axis, short value, float lx, float ly, float rx, float ry)
        {
            AxisChangedEventHandler raiseEvent = RaiseAxisChangedEvent;

            MyAxisEventArgs leftEventArgs = new()
            {
                Axis = axis,
                Value = value,
                X = lx,
                Y = ly
            };

            MyAxisEventArgs rightEventArgs = new()
            {
                Axis = axis,
                Value = value,
                X = rx,
                Y = ry
            };

            raiseEvent?.Invoke(this, leftEventArgs, rightEventArgs);
        }

        protected virtual void OnRaiseRightAxisChangedEvent(byte axis, short value, float x, float y)
        {
            RightAxisChangedEventHandler raiseEvent = RaiseRightAxisChangedEvent;

            MyAxisEventArgs eventArgs = new()
            {
                Axis = axis,
                Value = value,
                X = x,
                Y = y
            };

            raiseEvent?.Invoke(this, eventArgs);
        }

        protected virtual void OnRaiseLeftAxisChangedEvent(byte axis, short value, float x, float y)
        {
            LeftAxisChangedEventHandler raiseEvent = RaiseLeftAxisChangedEvent;

            MyAxisEventArgs eventArgs = new()
            {
                 Axis = axis,
                 Value = value,
                 X = x,
                 Y = y
            };

            raiseEvent?.Invoke(this, eventArgs);
        }

        protected virtual void OnRaiseLeftTriggerChangedEvent(float value)
        {
            LeftTriggerChangedEventHandler raiseEvent = RaiseLeftTriggerChangedEvent;

            MyTriggerEventArgs eventArgs = new()
            {
                Value = value
            };

            raiseEvent?.Invoke(this, eventArgs);
        }

        protected virtual void OnRaiseRightTriggerChangedEvent(float value)
        {
            RightTriggerChangedEventHandler raiseEvent = RaiseRightTriggerChangedEvent;

            MyTriggerEventArgs eventArgs = new()
            {
                Value = value
            };

            raiseEvent?.Invoke(this, eventArgs);
        }

        protected virtual void OnRaiseButtonChangedEvent(byte button, bool pressed)
        {
            ButtonChangedEventHandler raiseEvent = RaiseButtonChangedEvent;

            MyButtonEventArgs eventArgs = new()
            {
                 Button = button,
                 Pressed = pressed
            };

            raiseEvent?.Invoke(this, eventArgs);
        }

        #endregion

    }
}
