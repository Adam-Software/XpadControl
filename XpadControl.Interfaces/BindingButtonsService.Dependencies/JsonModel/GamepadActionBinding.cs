using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace XpadControl.Interfaces.BindingButtonsService.Dependencies.JsonModel
{
    public class GamepadActionBinding
    {
        #region Buttons action

        /// <summary>
        /// All buttons with linked actions
        /// </summary>
        public List<ButtonActionBinding> Buttons { get; private set; } = new List<ButtonActionBinding>();

        private List<ButtonActionBinding> mButtonsAction;

        [JsonPropertyName("action_buttons")]
        public List<ButtonActionBinding> ButtonsAction 
        { 
            get { return mButtonsAction; }
            set 
            {
                if (value == null)
                    return;

                mButtonsAction = value; 
                Buttons.AddRange(ButtonsAction);   
            }
        }

        private List<ButtonActionBinding> mButtonsOption;

        [JsonPropertyName("option_buttons")]
        public List<ButtonActionBinding> ButtonsOption 
        {
            get { return mButtonsOption; }
            set
            {
                if (value == null)
                    return;

                mButtonsOption = value;
                Buttons.AddRange(ButtonsOption);
            }
        }

        private List<ButtonActionBinding> mButtonsDpad;

        [JsonPropertyName("dpad_buttons")]
        public List<ButtonActionBinding> ButtonsDpad
        {
            get { return mButtonsDpad; }
            set 
            {
                if (value == null)
                    return;

                mButtonsDpad = value;
                Buttons.AddRange(ButtonsDpad);
            }
        }

        private List<ButtonActionBinding> mButtonsStick;

        [JsonPropertyName("stick_buttons")]
        public List<ButtonActionBinding> ButtonsStick
        {
            get { return mButtonsStick; }
            set 
            {
                if (value == null)
                    return;

                mButtonsStick = value;
                Buttons.AddRange(ButtonsStick);
            }
        }

        private List<ButtonActionBinding> mButtonsBumper;

        [JsonPropertyName("bumper_buttons")]
        public List<ButtonActionBinding> ButtonsBumper
        {
            get { return mButtonsBumper; }
            set
            {
                if (value == null)
                    return;

                mButtonsBumper = value;
                Buttons.AddRange(ButtonsBumper);    
            }
        }

        #endregion

        #region Sticks & Trigger actions

        [JsonPropertyName("sticks")]
        public List<SticksActionBinding> SticksAction { get; set; }


        [JsonPropertyName("triggers")]
        public List<TriggerActionBinding> TriggerAction { get; set; }

        #endregion
    }
}
