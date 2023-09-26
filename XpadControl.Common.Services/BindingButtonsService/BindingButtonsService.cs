using System.Collections.Generic;
using XpadControl.Interfaces;
using XpadControl.Interfaces.BindingButtonsService.Dependencies;
using XpadControl.Interfaces.BindingButtonsService.Dependencies.JsonModel;

namespace XpadControl.Common.Services.BindingButtonsService
{
    public class BindingButtonsService : IBindingButtonsService
    {
        private readonly GamepadActionBinding mGamepadActionBinding;
        public BindingButtonsService(ILoggerService loggerService, string jsonConfigPath) 
        {
            mGamepadActionBinding = jsonConfigPath.ToGamepadAction();

            loggerService.WriteInformationLog($"Init gamepad binding service");
        }

        public List<ButtonActionBinding> ButtonBindings => mGamepadActionBinding.ButtonBindings;

        public List<SticksToActionBindingModel> SticksActions => mGamepadActionBinding.SticksActions;

        public List<TriggerToActionBindingModel> TriggerAction => mGamepadActionBinding.TriggerActions;
    }
}
