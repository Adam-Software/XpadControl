using System;
using System.Collections.Generic;
using System.Linq;
using XpadControl.Interfaces;
using XpadControl.Interfaces.BindingButtonsService.Dependencies;
using XpadControl.Interfaces.BindingButtonsService.Dependencies.EventArgs;
using XpadControl.Interfaces.BindingButtonsService.Dependencies.JsonModel;
using XpadControl.Interfaces.GamepadService.Dependencies.EventArgs;

namespace XpadControl.Common.Services.BindingButtonsService
{
    public class BindingButtonsService : IBindingButtonsService
    {
        public event ActionEventHandler RaiseActionEvent;

        private List<ButtonActionBinding> mButtonBindings;
        private List<SticksActionBinding> mSticksActions;
        private List<TriggerActionBinding> mTriggerAction;

        public BindingButtonsService(ILoggerService loggerService, IGamepadService gamepadService, string jsonConfigPath) 
        {
            var gamepadActionBinding = jsonConfigPath.ToGamepadAction();

            mButtonBindings = gamepadActionBinding.ButtonsAction;
            mSticksActions = gamepadActionBinding.SticksAction;
            mTriggerAction = gamepadActionBinding.TriggerAction;

            gamepadService.RaiseLeftTriggerChangedEvent += RaiseLeftTriggerChangedEvent;
            gamepadService.RaiseRightTriggerChangedEvent += RaiseRightTriggerChangedEvent;

            gamepadService.RaiseAxisChangedEvent += RaiseAxisChangedEvent;
            gamepadService.RaiseButtonChangedEvent += RaiseButtonChangedEvent;
        }
      
        private void RaiseButtonChangedEvent(object sender, ButtonEventArgs e)
        {
            var pressedButton = e.Button;
            var isPressed = e.Pressed == true;

            AdamActions action = mButtonBindings
                .Where(x => x.Button == pressedButton.ToConfigButton())
                .Select(x => x.Action).FirstOrDefault();

            ActionEventArgs eventArgs = new ActionEventArgs
            {
                AdamActions = action,
                IsAxis = false,
                IsButton = true,
                IsTrigger = false,
                
                ButtonValue = e.Pressed,
                FloatValue = 0,
            };

            OnRaiseActionEvent(eventArgs);
        }

        private void RaiseAxisChangedEvent(object sender, AxisEventArgs left, AxisEventArgs right)
        {
            left.RaiseAxisPropertyChangedEvent += LeftAxisPropertyChangedEvent;
            right.RaiseAxisPropertyChangedEvent += RightAxisPropertyChangedEvent;
        }

        private void RightAxisPropertyChangedEvent(object sender, AxisPropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "X":
                    AdamActions actionX = mSticksActions.Where(x => x.Sticks == Sticks.RightStickX).Select(x => x.Action).FirstOrDefault();
                    
                    ActionEventArgs eventArgsX = new ActionEventArgs
                    {
                        AdamActions = actionX,
                        IsAxis = true,
                        IsButton = false,
                        IsTrigger = false,

                        ButtonValue = false,
                        FloatValue = e.Value,
                    };

                    OnRaiseActionEvent(eventArgsX);
                    break;

                case "Y":
                    AdamActions actionY = mSticksActions.Where(x => x.Sticks == Sticks.RightStickY).Select(x => x.Action).FirstOrDefault();
                    
                    ActionEventArgs eventArgsY = new ActionEventArgs
                    {
                        AdamActions = actionY,
                        IsAxis = true,
                        IsButton = false,
                        IsTrigger = false,

                        ButtonValue = false,
                        FloatValue = e.Value,
                    };

                    OnRaiseActionEvent(eventArgsY);
                    break;
            }
        }

        private void LeftAxisPropertyChangedEvent(object sender, AxisPropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "X":
                    AdamActions actionX = mSticksActions.Where(x => x.Sticks == Sticks.LeftStickX).Select(x => x.Action).FirstOrDefault();
                    
                    ActionEventArgs eventArgsX = new ActionEventArgs
                    {
                        AdamActions = actionX,
                        IsAxis = true,
                        IsButton = false,
                        IsTrigger = false,

                        ButtonValue = false,
                        FloatValue = e.Value,
                    };

                    OnRaiseActionEvent(eventArgsX);
                    break;

                case "Y":
                    AdamActions actionY = mSticksActions.Where(x => x.Sticks == Sticks.LeftStickY).Select(x => x.Action).FirstOrDefault();
                    
                    ActionEventArgs eventArgsY = new ActionEventArgs
                    {
                        AdamActions = actionY,
                        IsAxis = true,
                        IsButton = false,
                        IsTrigger = false,

                        ButtonValue = false,
                        FloatValue = e.Value,
                    };

                    OnRaiseActionEvent(eventArgsY);
                    break;
            }
        }

        private void RaiseRightTriggerChangedEvent(object sender, TriggerEventArgs e)
        {
            AdamActions action = mTriggerAction.Where(x => x.Trigger == Triggers.RightTrigger).Select(x => x.Action).FirstOrDefault();

            ActionEventArgs eventArgs = new ActionEventArgs
            {
                AdamActions = action,
                IsAxis = false,
                IsButton = false,
                IsTrigger = true,

                ButtonValue = false,
                FloatValue = e.Value,
            };

            OnRaiseActionEvent(eventArgs);
        }

        private void RaiseLeftTriggerChangedEvent(object sender, TriggerEventArgs e)
        {
            AdamActions action = mTriggerAction.Where(x => x.Trigger == Triggers.LeftTrigger).Select(x => x.Action).FirstOrDefault();
            
            ActionEventArgs eventArgs = new ActionEventArgs
            {
                AdamActions = action,
                IsAxis = false,
                IsButton = false,
                IsTrigger = true,

                ButtonValue = false,
                FloatValue = e.Value,
            };
        }

        protected virtual void OnRaiseActionEvent(ActionEventArgs aventArgs)
        {
            ActionEventHandler raiseEvent = RaiseActionEvent;

            raiseEvent?.Invoke(this, aventArgs);
        }
    }
}
