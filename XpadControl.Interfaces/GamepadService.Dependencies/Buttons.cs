namespace XpadControl.Interfaces.GamepadService.Dependencies
{
    /// <summary>
    /// Buttons value 
    /// 
    /// Copy from https://github.com/AderitoSilva/XInputium/blob/main/source/XInputium/XInputium/XInput/XButtons.cs
    /// </summary>
    public enum Buttons
    {

        // No button. This is used to represent no buttons.
        None,

        // D-Pad Up. This is one of the directional buttons.
        DPadUp,


        // D-Pad Down. This is one of the directional buttons.
        DPadDown,
        
        // D-Pad Left. This is one of the directional buttons.
        DPadLeft,
        
        // D-Pad Right. This is one of the directional buttons.
        DPadRight,
        
        // The Start button.
        Start,
        
        // The Back button.
        Back,
        
        // The LS (Left Stick) button.
        LS,
        
        // The RS (Right Stick) button.
        RS,
        
        // The LB (Left Shoulder) button.
        LB,
        
        // The RB (Right Shoulder).
        RB,
        
        // The A button.
        A,
        
        // The B button.
        B,
        
        // The X button.
        X,
        
        // The Y button.
        Y
    }
}
