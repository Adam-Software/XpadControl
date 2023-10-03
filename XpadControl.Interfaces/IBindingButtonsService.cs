using System;
using XpadControl.Interfaces.BindingButtonsService.Dependencies.EventArgs;

namespace XpadControl.Interfaces
{
    #region action delegate

    public delegate void ActionEventHandler(object sender, ActionEventArgs eventArgs);

    #endregion

    public interface IBindingButtonsService : IDisposable
    {
        public event ActionEventHandler RaiseActionEvent;
    }
}
