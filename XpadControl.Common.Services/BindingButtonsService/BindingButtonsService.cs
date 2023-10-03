using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
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

        private static List<ButtonActionBinding> mButtonBindings;

        private readonly AdamActions mActionLX;
        private readonly AdamActions mActionLY;
        private readonly AdamActions mActionRX;
        private readonly AdamActions mActionRY;
        private readonly AdamActions mActionRightTrigger;
        private readonly AdamActions mActionLeftTrigger;

        private const string commonErrorMessage = $"An error occurred in the {nameof(BindingButtonsService)}";

        public BindingButtonsService(ILoggerService loggerService, IGamepadService gamepadService, string jsonConfigPath) 
        {
            mGamepadService = gamepadService;
            GamepadActionBinding actionBinding = new();

            try
            {
                actionBinding = jsonConfigPath.ToGamepadAction();
            }
            catch(JsonException ex) 
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

            mButtonBindings = actionBinding.Buttons;

            mActionLX = actionBinding.SticksAction.Where(x => x.Sticks == ConfigSticks.RightStickX).Select(x => x.Action).FirstOrDefault();
            mActionLY = actionBinding.SticksAction.Where(x => x.Sticks == ConfigSticks.RightStickY).Select(x => x.Action).FirstOrDefault();

            mActionRX = actionBinding.SticksAction.Where(x => x.Sticks == ConfigSticks.LeftStickX).Select(x => x.Action).FirstOrDefault();
            mActionRY = actionBinding.SticksAction.Where(x => x.Sticks == ConfigSticks.LeftStickY).Select(x => x.Action).FirstOrDefault();

            mActionRightTrigger = actionBinding.TriggerAction.Where(x => x.Trigger == ConfigTriggers.RightTrigger).Select(x => x.Action).FirstOrDefault();
            mActionLeftTrigger = actionBinding.TriggerAction.Where(x => x.Trigger == ConfigTriggers.LeftTrigger).Select(x => x.Action).FirstOrDefault();

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
            ConfigButtons pressedButton = e.Button.ToConfigButton();
            AdamActions action = mButtonBindings.Where(x => x.Button == pressedButton).Select(x => x.Action).FirstOrDefault();

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
                    
                    ActionEventArgs eventArgsX = new()
                    {
                        AdamActions = mActionLX,
                        IsAxis = IsAxis.IsAxisX,
                        FloatValue = eventArgs.X
                    };

                    OnRaiseActionEvent(eventArgsX);
                    break;

                case AxisPropertyChanged.Y:
  
                    ActionEventArgs eventArgsY = new()
                    {
                        AdamActions = mActionLY,
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
                
                    ActionEventArgs eventArgsX = new()
                    {
                        AdamActions = mActionRX,
                        IsAxis = IsAxis.IsAxisX,
                        FloatValue = eventArgs.X
                    };

                    OnRaiseActionEvent(eventArgsX);
                    break;

                case AxisPropertyChanged.Y:

                    ActionEventArgs eventArgsY = new()
                    {
                        AdamActions = mActionRY,
                        IsAxis = IsAxis.IsAxisY,
                        FloatValue = eventArgs.Y
                    };

                    OnRaiseActionEvent(eventArgsY);
                    break;
            }
        }

        private void RaiseRightTriggerChangedEvent(object sender, TriggerEventArgs e)
        {
            ActionEventArgs eventArgs = new()
            {
                AdamActions = mActionRightTrigger,
                IsTrigger = IsTrigger.IsTriggerRight,
                FloatValue = e.Value,
            };

            OnRaiseActionEvent(eventArgs);
        }

        private void RaiseLeftTriggerChangedEvent(object sender, TriggerEventArgs e)
        {
            ActionEventArgs eventArgs = new()
            {
                AdamActions = mActionLeftTrigger,
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
