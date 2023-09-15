using Gamepad;
using System;
using XpadControl.Interfaces.GamepadService;
using XpadControl.Interfaces.LoggerService;
using XpadControl.Interfaces.GamepadService.Dependencies.Extensions;
using XpadControl.Interfaces.GamepadService.Dependencies;
using XpadControl.Linux.Services.Extensions;

using MyAxisEventArgs = XpadControl.Interfaces.GamepadService.Dependencies.EventArgs.AxisEventArgs;
using MyButtonEventArgs = XpadControl.Interfaces.GamepadService.Dependencies.EventArgs.ButtonEventArgs;
using MyTriggerEventArgs = XpadControl.Interfaces.GamepadService.Dependencies.EventArgs.TriggerEventArgs;
using MyConnectedEventArgs = XpadControl.Interfaces.GamepadService.Dependencies.EventArgs.ConnectedEventArgs;
using System.IO;


namespace XpadControl.Linux.Services.GamepadService
{
    public class GamepadService : IGamepadService
    {
        public event AxisChangedEventHandler RaiseAxisChangedEvent;

        public event LeftTriggerChangedEventHandler RaiseLeftTriggerChangedEvent;
        public event RightTriggerChangedEventHandler RaiseRightTriggerChangedEvent;

        public event ButtonChangedEventHandler RaiseButtonChangedEvent;
        public event ConnectedChangedEventHandler RaiseConnectedChangedEvent;

        private GamepadController? mGamepad;
        private readonly ILoggerService mLoggerService;

        public GamepadService(ILoggerService loggerService) 
        {
            mLoggerService = loggerService;

            mLoggerService.WriteVerboseLog("OS is Linux");   
        }

        public void Update() 
        {
            if (File.Exists("/dev/input/js0"))
            {
                try
                {
                    GamepadConnected();
                }
                catch (Exception ex) 
                {
                    mLoggerService.WriteErrorLog($"{ex.Message}");
                }
            }
                

            if (!File.Exists("/dev/input/js0"))
            {
                try
                {
                    GamepadDisconnected();
                }
                catch (Exception ex)
                {
                    mLoggerService.WriteErrorLog($"{ex.Message}");
                }
            }
        }

        #region Connect/Disconect gamepad methods

        private void GamepadConnected()
        {
            if (mGamepad == null)
            {
                mGamepad = new GamepadController("/dev/input/js0");

                mGamepad.AxisChanged += AxisChanged;
                mGamepad.ButtonChanged += ButtonChanged;

                OnRaiseConnectedChangedEvent(true);
                mLoggerService.WriteVerboseLog("Gamepad connected");
            }
        }

        private void GamepadDisconnected()
        {
            if (mGamepad != null)
            {
                mGamepad.AxisChanged -= AxisChanged;
                mGamepad.ButtonChanged -= ButtonChanged;
                mGamepad.Dispose();

                mGamepad = null;

                OnRaiseConnectedChangedEvent(false);
                mLoggerService.WriteVerboseLog("Gamepad disconnected");
            }
        }

        #endregion

        public void Dispose()
        {
            mLoggerService.WriteVerboseLog($"Dispose {nameof(GamepadService)} called");

            mGamepad?.Dispose();
        }

        #region Gamepad event

        private void ButtonChanged(object sender, ButtonEventArgs e)
        {
            mLoggerService.WriteVerboseLog($"{e.Button} is {e.Pressed}");

            OnRaiseButtonChangedEvent(e.Button.ToButtons(), e.Pressed);
        }

        private float lx = 0, ly = 0, rx = 0, ry = 0;
        private float leftTrigger, rightTrigger = 0;

        private void AxisChanged(object sender, AxisEventArgs e)
        {
            byte axis = e.Axis;
            short value = e.Value;

            mLoggerService.WriteVerboseLog($"{axis} is {value}");

            switch (axis)
            {
                case 0:
                    lx = value.AxisToFloat();
                    OnRaiseAxisChangedEvent(lx, ly, rx, ry);

                    mLoggerService.WriteVerboseLog($"LEFT STICK X:{lx} or {value}");
                    break;
                case 2:
                    leftTrigger = value.TriggerToFloat();
                    OnRaiseLeftTriggerChangedEvent(leftTrigger);
                    
                    mLoggerService.WriteVerboseLog($"LEFT TRIGGER value is {value}");
                    break;
                case 1:
                    ly = -value.AxisToFloat();
                    OnRaiseAxisChangedEvent(lx, ly, rx, ry);

                    mLoggerService.WriteVerboseLog($"LEFT STICK Y:{ly} or {value}");
                    break; 
                case 3:
                    rx = value.AxisToFloat();
                    OnRaiseAxisChangedEvent(lx, ly, rx, ry);

                    mLoggerService.WriteVerboseLog($"RIGHT STICK X:{rx} or {value}");
                    break;
                case 4:
                    ry = -value.AxisToFloat();
                    OnRaiseAxisChangedEvent(lx, ly, rx, ry);

                    mLoggerService.WriteVerboseLog($"RIGHT STICK Y:{ry} or {value}");
                    break;
                case 5:
                    rightTrigger = value.TriggerToFloat();
                    OnRaiseRightTriggerChangedEvent(rightTrigger);
                    
                    mLoggerService.WriteVerboseLog($"RIGHT TRIGGER value is {value}");
                    break;
                case 6:
                    value.ToButtonEventArgs(true);

                    mLoggerService.WriteVerboseLog($"X DPAD value is {value}");
                    break;
                case 7:
                    value.ToButtonEventArgs(false);

                    mLoggerService.WriteVerboseLog($"Y DPAD value is {value}");
                    break;
            }
        }

        #endregion


        #region Raise events

        protected virtual void OnRaiseAxisChangedEvent(float lx, float ly, float rx, float ry)
        {
            AxisChangedEventHandler raiseEvent = RaiseAxisChangedEvent;

            MyAxisEventArgs leftEventArgs = new()
            {
                X = lx,
                Y = ly
            };

            MyAxisEventArgs rightEventArgs = new()
            {
                X = rx,
                Y = ry
            };

            raiseEvent?.Invoke(this, leftEventArgs, rightEventArgs);
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

        protected virtual void OnRaiseButtonChangedEvent(Buttons button, bool pressed)
        {
            ButtonChangedEventHandler raiseEvent = RaiseButtonChangedEvent;

            MyButtonEventArgs eventArgs = new()
            {
                 Button = button,
                 Pressed = pressed
            };

            raiseEvent?.Invoke(this, eventArgs);
        }

        protected virtual void OnRaiseButtonChangedEvent(MyButtonEventArgs eventArgs)
        {
            ButtonChangedEventHandler raiseEvent = RaiseButtonChangedEvent;

            raiseEvent?.Invoke(this, eventArgs);
        }

        protected virtual void OnRaiseConnectedChangedEvent(bool isConnected)
        {
            ConnectedChangedEventHandler raiseEvent = RaiseConnectedChangedEvent;

            MyConnectedEventArgs eventArgs = new()
            {
                IsConnected = isConnected
            };

            raiseEvent?.Invoke(this, eventArgs);
        }

        #endregion

    }
}
