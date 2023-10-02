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

        private readonly List<ButtonActionBinding> mButtonBindings;
        private readonly List<SticksActionBinding> mSticksActions;
        private readonly List<TriggerActionBinding> mTriggerAction;

        public BindingButtonsService(ILoggerService loggerService, IGamepadService gamepadService, string jsonConfigPath) 
        {
            GamepadActionBinding gamepadActionBinding = jsonConfigPath.ToGamepadAction();

            List<ButtonActionBinding> buttonBindings = gamepadActionBinding.ButtonsAction;
            buttonBindings.AddRange(gamepadActionBinding.SticksButtonsAction);
            buttonBindings.AddRange(gamepadActionBinding.ButtonsBumper);
            buttonBindings.AddRange(gamepadActionBinding.ButtonsDpad);
            buttonBindings.AddRange(gamepadActionBinding.ButtonsOption);

            mButtonBindings = buttonBindings;
            mSticksActions = gamepadActionBinding.SticksAction;
            mTriggerAction = gamepadActionBinding.TriggerAction;

            gamepadService.RaiseLeftTriggerChangedEvent += RaiseLeftTriggerChangedEvent;
            gamepadService.RaiseRightTriggerChangedEvent += RaiseRightTriggerChangedEvent;

            gamepadService.RaiseLeftAxisChangedEvent += RaiseLeftAxisChangedEvent;
            gamepadService.RaiseRightAxisChangedEvent += RaiseRightAxisChangedEvent;
            
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
                IsButton = e.Pressed.ToButtonEventArgs(),
            };

            OnRaiseActionEvent(eventArgs);
        }

        private void RaiseRightAxisChangedEvent(object sender, AxisPropertyChanged axisChanged, AxisEventArgs eventArgs)
        {
            switch (axisChanged)
            {
                case AxisPropertyChanged.X:
                    AdamActions actionX = mSticksActions.Where(x => x.Sticks == ConfigSticks.RightStickX).Select(x => x.Action).FirstOrDefault();

                    ActionEventArgs eventArgsX = new ActionEventArgs
                    {
                        AdamActions = actionX,
                        IsAxis = IsAxis.IsAxisX,
                        FloatValue = eventArgs.X
                    };

                    OnRaiseActionEvent(eventArgsX);
                    break;

                case AxisPropertyChanged.Y:
                    AdamActions actionY = mSticksActions.Where(x => x.Sticks == ConfigSticks.RightStickY).Select(x => x.Action).FirstOrDefault();

                    ActionEventArgs eventArgsY = new ActionEventArgs
                    {
                        AdamActions = actionY,
                        IsAxis = IsAxis.IsAxisY,
                        FloatValue = eventArgs.Y
                    };

                    OnRaiseActionEvent(eventArgsY);
                    break;
            }
        }

        private void RaiseLeftAxisChangedEvent(object sender, AxisPropertyChanged axisChanged, AxisEventArgs eventArgs)
        {
            switch (axisChanged)
            {
                case AxisPropertyChanged.X:
                    AdamActions actionX = mSticksActions.Where(x => x.Sticks == ConfigSticks.LeftStickX).Select(x => x.Action).FirstOrDefault();

                    ActionEventArgs eventArgsX = new ActionEventArgs
                    {
                        AdamActions = actionX,
                        IsAxis = IsAxis.IsAxisX,
                        FloatValue = eventArgs.X
                    };

                    OnRaiseActionEvent(eventArgsX);
                    break;

                case AxisPropertyChanged.Y:
                    AdamActions actionY = mSticksActions.Where(x => x.Sticks == ConfigSticks.LeftStickY).Select(x => x.Action).FirstOrDefault();

                    ActionEventArgs eventArgsY = new ActionEventArgs
                    {
                        AdamActions = actionY,
                        IsAxis = IsAxis.IsAxisY,
                        FloatValue = eventArgs.Y
                    };

                    OnRaiseActionEvent(eventArgsY);
                    break;
            }
        }


        private void RightAxisPropertyChangedEvent(object sender, AxisPropertyChangedEventArgs e)
        {

        }

        private void RaiseRightTriggerChangedEvent(object sender, TriggerEventArgs e)
        {
            AdamActions action = mTriggerAction.Where(x => x.Trigger == ConfigTriggers.RightTrigger).Select(x => x.Action).FirstOrDefault();

            ActionEventArgs eventArgs = new ActionEventArgs
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
            
            ActionEventArgs eventArgs = new ActionEventArgs
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
