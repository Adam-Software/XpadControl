using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace XpadControl.Interfaces.BindingButtonsService.Dependencies
{
    /// <summary>
    /// The values are used for quick conversion to config buttons. Dont change this!
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumMemberConverter))]
    public enum ConfigButtons
    {
        None = 0x0,
        // D-Pad Up. This is one of the directional buttons.
        DPadUp = 0x1,

        // D-Pad Down. This is one of the directional buttons.
        DPadDown = 0x2,

        // D-Pad Left. This is one of the directional buttons.
        DPadLeft = 0x4,

        // D-Pad Right. This is one of the directional buttons.
        DPadRight = 0x8,

        [EnumMember(Value = "button_start")]
        Start = 0x10,

        [EnumMember(Value = "button_back")]
        Back = 0x20,

        // The LS (Left Stick) button.
        LS = 0x40,

        // The RS (Right Stick) button.
        RS = 0x80,

        // The LB (Left Shoulder) button.
        LB = 0x100,

        // The RB (Right Shoulder).
        RB = 0x200,

        [EnumMember(Value = "button_a")]
        A = 0x1000,

        [EnumMember(Value = "button_b")]
        B = 0x2000,

        [EnumMember(Value = "button_x")]
        X = 0x4000,

        [EnumMember(Value = "button_y")]
        Y = 0x8000




    }
}
