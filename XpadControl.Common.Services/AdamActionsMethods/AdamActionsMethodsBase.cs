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
                throw new NotImplementedException("Button can`t binding to axis value");
            }

            if (eventArgs.IsAxis != IsAxis.None)
            {
                HeadUpDown(eventArgs.FloatValue);
            }

            if (eventArgs.IsTrigger != IsTrigger.None)
            {
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
                float floatValue = eventArgs.FloatValue / 2;
                int intValue = (int) Math.Round(floatValue);

                HeadDown(intValue);
            }

            if (eventArgs.IsTrigger != IsTrigger.None)
            {
                float floatValue = eventArgs.FloatValue / 2;
                int intValue = (int)Math.Round(floatValue);

                HeadDown(intValue);
            }
        }

        public abstract void HeadDown(int value);
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
                float floatValue = eventArgs.FloatValue / 2;
                int intValue = (int) Math.Round(floatValue);

                HeadUp(intValue);
            }

            if (eventArgs.IsTrigger != IsTrigger.None)
            {
                float floatValue = eventArgs.FloatValue / 2;
                int intValue = (int)Math.Round(floatValue);

                HeadUp(intValue);
            }
        }

        public abstract void HeadUp(int value);
        public abstract void HeadUp(bool value);

        #endregion

        #endregion
    }
}
