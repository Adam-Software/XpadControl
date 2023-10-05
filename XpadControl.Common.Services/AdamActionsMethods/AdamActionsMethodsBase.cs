using System;
using XpadControl.Interfaces.BindingButtonsService.Dependencies.EventArgs;

namespace XpadControl.Common.Services.AdamActionsMethods
{
    /// <summary>
    /// Here it is determined under what conditions the command implementation call is launched
    /// </summary>
    public abstract class AdamActionsMethodsBase
    {
        #region ToHomePosition

        public virtual void ToHomePosition(ActionEventArgs eventArgs)
        {
            if (eventArgs.IsButton == IsButton.IsButtonPressed)
            {
                HomePositionCommandExecute();
            }

            if (eventArgs.IsAxis != IsAxis.None)  
            {
                if (eventArgs.FloatValue <= -0.5 || eventArgs.FloatValue >= 0.5)
                {
                    HomePositionCommandExecute();
                }
            }

            if (eventArgs.IsTrigger != IsTrigger.None)
            {
                if (eventArgs.FloatValue >= 0.5)
                {
                    HomePositionCommandExecute();
                }
            }
        }

        public abstract void HomePositionCommandExecute();

        #endregion

        #region HeadUpDown/HeadUp/HeadDown

        #region HeadUpDown

        public virtual void HeadUpDown(ActionEventArgs eventArgs)
        {
            if (eventArgs.IsButton == IsButton.IsButtonPressed)
            {
                throw new NotImplementedException("Button can`t binding to axis action");
            }

            if (eventArgs.IsAxis != IsAxis.None)
            {
                HeadUpDown(eventArgs.FloatValue);
            }

            if (eventArgs.IsTrigger != IsTrigger.None)
            {
                throw new NotImplementedException("Trigger can`t binding to axis action");
            }
        }

        public abstract void HeadUpDown(float value);

        #endregion

        #region HeadDown

        public virtual void HeadDown(ActionEventArgs eventArgs)
        {
            if (eventArgs.IsButton == IsButton.IsButtonPressed)
            {
                HeadDown(eventArgs.IsButton == IsButton.IsButtonPressed);
            }

            if (eventArgs.IsAxis != IsAxis.None)
            {
                HeadDown(eventArgs.FloatValue);
            }

            if (eventArgs.IsTrigger != IsTrigger.None)
            {
                HeadDown(eventArgs.FloatValue);
            }
        }

        public abstract void HeadDown(float value);
        public abstract void HeadDown(bool value);

        #endregion

        #region HeadUp

        public virtual void HeadUp(ActionEventArgs eventArgs)
        {
            if (eventArgs.IsButton != IsButton.None)
            {
                HeadUp(eventArgs.IsButton == IsButton.IsButtonPressed);
            }

            if (eventArgs.IsAxis != IsAxis.None)
            {
                HeadUp(eventArgs.FloatValue);
            }

            if (eventArgs.IsTrigger != IsTrigger.None)
            {
                HeadUp(eventArgs.FloatValue);
            }
        }

        public abstract void HeadUp(float value);
        public abstract void HeadUp(bool value);

        #endregion

        #endregion
    }
}
