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

        [EnumMember(Value = "dpad.up")]
        DPadUp = 0x1,

        [EnumMember(Value = "dpad.down")]
        DPadDown = 0x2,

        [EnumMember(Value = "dpad.left")]
        DPadLeft = 0x4,

        [EnumMember(Value = "dpad.right")]
        DPadRight = 0x8,

        [EnumMember(Value = "button.start")]
        Start = 0x10,

        [EnumMember(Value = "button.back")]
        Back = 0x20,

        [EnumMember(Value = "left.stick.button")]
        LS = 0x40,

        [EnumMember(Value = "right.stick.button")]
        RS = 0x80,

        [EnumMember(Value = "left.bamper")]
        LB = 0x100,

        [EnumMember(Value = "right.bamper")]
        RB = 0x200,

        [EnumMember(Value = "button.a")]
        A = 0x1000,

        [EnumMember(Value = "button.b")]
        B = 0x2000,

        [EnumMember(Value = "button.x")]
        X = 0x4000,

        [EnumMember(Value = "button.y")]
        Y = 0x8000
    }
}
