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
        None = 0x0,

        // D-Pad Up. This is one of the directional buttons.
        DPadUp = 0x1,


        // D-Pad Down. This is one of the directional buttons.
        DPadDown = 0x2,

        // D-Pad Left. This is one of the directional buttons.
        DPadLeft = 0x4,

        // D-Pad Right. This is one of the directional buttons.
        DPadRight = 0x8,

        // The Start button.
        Start = 0x10,

        // The Back button.
        Back = 0x20,

        // The LS (Left Stick) button.
        LS = 0x40,

        // The RS (Right Stick) button.
        RS = 0x80,

        // The LB (Left Shoulder) button.
        LB = 0x100,

        // The RB (Right Shoulder).
        RB = 0x200,

        // The A button.
        A = 0x1000,

        // The B button.
        B = 0x2000,

        // The X button.
        X = 0x4000,

        // The Y button.
        Y = 0x8000
    }
}
