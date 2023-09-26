using System.Collections.Generic;
using XpadControl.Interfaces.BindingButtonsService.Dependencies.JsonModel;

namespace XpadControl.Interfaces
{
    public interface IBindingButtonsService
    {
        public List<ButtonActionBinding> ButtonBindings { get; }

        public List<SticksToActionBindingModel> SticksActions { get; }

        public List<TriggerToActionBindingModel> TriggerAction { get; }

    }
}
