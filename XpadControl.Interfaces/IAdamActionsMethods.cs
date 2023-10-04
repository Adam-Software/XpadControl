using XpadControl.Interfaces.BindingButtonsService.Dependencies.EventArgs;

namespace XpadControl.Interfaces
{
    public interface IAdamActionsMethods
    {
        public void ToHomePosition(ActionEventArgs eventArgs);

        #region HeadUpDown/HeadUp/HeadDown
        public void HeadUpDown(ActionEventArgs eventArgs);
        public void HeadUp(ActionEventArgs eventArgs);
        public void HeadDown(ActionEventArgs eventArgs);

        #endregion
    }
}
