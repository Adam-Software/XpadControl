using System;
using System.Collections.Generic;
using System.Linq;
using XpadControl.Interfaces;
using XpadControl.Interfaces.BindingButtonsService.Dependencies;
using XpadControl.Interfaces.BindingButtonsService.Dependencies.EventArgs;
using XpadControl.Interfaces.BindingButtonsService.Dependencies.JsonModel;
using XpadControl.Interfaces.GamepadService.Dependencies.EventArgs;
using XpadControl.Interfaces.GamepadService.Dependencies.EventArgs.PropertyChangedArgs;


namespace XpadControl.Common.Services.BindingButtonsService
{
    public class BindingButtonsService : IBindingButtonsService
    {
        public event ActionEventHandler RaiseActionEvent;

        private readonly IGamepadService mGamepadService;
        private readonly List<ButtonActionBinding> mButtonBindings;
        private readonly List<SticksActionBinding> mSticksActions;
        private readonly List<TriggerActionBinding> mTriggerAction;

        public BindingButtonsService(ILoggerService loggerService, IGamepadService gamepadService, string jsonConfigPath) 
        {
            mGamepadService = gamepadService;

            const string commonErrorMessage = $"An error occurred in the {nameof(BindingButtonsService)}";
            GamepadActionBinding gamepadActionBinding = new();

            try
            {
                gamepadActionBinding = jsonConfigPath.ToGamepadAction();
            }
            catch(System.Text.Json.JsonException ex) 
            {
                loggerService.WriteErrorLog(commonErrorMessage);
                loggerService.WriteErrorLog($"Error reading json config {jsonConfigPath}");
                loggerService.WriteErrorLog($"Error details {ex.Message}");
            }
            catch (Exception ex)
            {
                loggerService.WriteErrorLog(commonErrorMessage);
                loggerService.WriteErrorLog($"{ex}");
            }

            mButtonBindings = gamepadActionBinding.Buttons;
            mSticksActions = gamepadActionBinding.SticksAction;
            mTriggerAction = gamepadActionBinding.TriggerAction;

            SubscribeToEvent();
        }

        public void Dispose()
        {
            UnsubscribeFromEvent();
        }

        #region Subscribe/Unsubscribe events

        private void SubscribeToEvent()
        {

            mGamepadService.RaiseLeftTriggerChangedEvent += RaiseLeftTriggerChangedEvent;
            mGamepadService.RaiseRightTriggerChangedEvent += RaiseRightTriggerChangedEvent;

            mGamepadService.RaiseLeftAxisChangedEvent += RaiseLeftAxisChangedEvent;
            mGamepadService.RaiseRightAxisChangedEvent += RaiseRightAxisChangedEvent;

            mGamepadService.RaiseButtonChangedEvent += RaiseButtonChangedEvent;
        }

        private void UnsubscribeFromEvent()
        {
            mGamepadService.RaiseLeftTriggerChangedEvent -= RaiseLeftTriggerChangedEvent;
            mGamepadService.RaiseRightTriggerChangedEvent -= RaiseRightTriggerChangedEvent;

            mGamepadService.RaiseLeftAxisChangedEvent -= RaiseLeftAxisChangedEvent;
            mGamepadService.RaiseRightAxisChangedEvent -= RaiseRightAxisChangedEvent;

            mGamepadService.RaiseButtonChangedEvent -= RaiseButtonChangedEvent;
        }

        #endregion

        private void RaiseButtonChangedEvent(object sender, ButtonEventArgs e)
        {
            var pressedButton = e.Button;
            var isPressed = e.Pressed == true;

            AdamActions action = mButtonBindings
                .Where(x => x.Button == pressedButton.ToConfigButton())
                .Select(x => x.Action).FirstOrDefault();

            ActionEventArgs eventArgs = new()
            {
                AdamActions = action,
                IsButton = e.Pressed.ToButtonEventArgs(),
            };

            OnRaiseActionEvent(eventArgs);
        }

        private void RaiseRightAxisChangedEvent(object sender, AxisEventArgs eventArgs)
        {
            switch (eventArgs.AxisPropertyChanged)
            {
                case AxisPropertyChanged.X:
                    AdamActions actionX = mSticksActions.Where(x => x.Sticks == ConfigSticks.RightStickX).Select(x => x.Action).FirstOrDefault();

                    ActionEventArgs eventArgsX = new()
                    {
                        AdamActions = actionX,
                        IsAxis = IsAxis.IsAxisX,
                        FloatValue = eventArgs.X
                    };

                    OnRaiseActionEvent(eventArgsX);
                    break;

                case AxisPropertyChanged.Y:
                    AdamActions actionY = mSticksActions.Where(x => x.Sticks == ConfigSticks.RightStickY).Select(x => x.Action).FirstOrDefault();

                    ActionEventArgs eventArgsY = new()
                    {
                        AdamActions = actionY,
                        IsAxis = IsAxis.IsAxisY,
                        FloatValue = eventArgs.Y
                    };

                    OnRaiseActionEvent(eventArgsY);
                    break;
            }
        }

        private void RaiseLeftAxisChangedEvent(object sender, AxisEventArgs eventArgs)
        {
            switch (eventArgs.AxisPropertyChanged)
            {
                case AxisPropertyChanged.X:
                    AdamActions actionX = mSticksActions.Where(x => x.Sticks == ConfigSticks.LeftStickX).Select(x => x.Action).FirstOrDefault();

                    ActionEventArgs eventArgsX = new()
                    {
                        AdamActions = actionX,
                        IsAxis = IsAxis.IsAxisX,
                        FloatValue = eventArgs.X
                    };

                    OnRaiseActionEvent(eventArgsX);
                    break;

                case AxisPropertyChanged.Y:
                    AdamActions actionY = mSticksActions.Where(x => x.Sticks == ConfigSticks.LeftStickY).Select(x => x.Action).FirstOrDefault();

                    ActionEventArgs eventArgsY = new()
                    {
                        AdamActions = actionY,
                        IsAxis = IsAxis.IsAxisY,
                        FloatValue = eventArgs.Y
                    };

                    OnRaiseActionEvent(eventArgsY);
                    break;
            }
        }

        private void RaiseRightTriggerChangedEvent(object sender, TriggerEventArgs e)
        {
            AdamActions action = mTriggerAction.Where(x => x.Trigger == ConfigTriggers.RightTrigger).Select(x => x.Action).FirstOrDefault();

            ActionEventArgs eventArgs = new()
            {
                AdamActions = action,
                IsTrigger = IsTrigger.IsTriggerRight,
                FloatValue = e.Value,
            };

            OnRaiseActionEvent(eventArgs);
        }

        private void RaiseLeftTriggerChangedEvent(object sender, TriggerEventArgs e)
        {
            AdamActions action = mTriggerAction.Where(x => x.Trigger == ConfigTriggers.LeftTrigger).Select(x => x.Action).FirstOrDefault();
            
            ActionEventArgs eventArgs = new()
            {
                AdamActions = action,
                IsTrigger = IsTrigger.IsTriggerLeft,
                FloatValue = e.Value
            };

            OnRaiseActionEvent(eventArgs);
        }

        protected virtual void OnRaiseActionEvent(ActionEventArgs aventArgs)
        {
            ActionEventHandler raiseEvent = RaiseActionEvent;

            raiseEvent?.Invoke(this, aventArgs);
        }
    }
}
