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
        [EnumMember(Value = "none")]
        None = 0x0,

        [EnumMember(Value = "dpad_up")]
        DPadUp = 0x1,

        [EnumMember(Value = "dpad_down")]
        DPadDown = 0x2,

        [EnumMember(Value = "dpad_left")]
        DPadLeft = 0x4,

        [EnumMember(Value = "dpad_right")]
        DPadRight = 0x8,

        [EnumMember(Value = "button_start")]
        Start = 0x10,

        [EnumMember(Value = "button_back")]
        Back = 0x20,

        // The LS (Left Stick) button.
        LS = 0x40,

        // The RS (Right Stick) button.
        RS = 0x80,

        [EnumMember(Value = "left_bamper")]
        LB = 0x100,

        [EnumMember(Value = "right_bamper")]
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
