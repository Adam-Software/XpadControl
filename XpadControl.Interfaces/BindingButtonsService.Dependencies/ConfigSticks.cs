using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace XpadControl.Interfaces.BindingButtonsService.Dependencies
{
    [JsonConverter(typeof(JsonStringEnumMemberConverter))]
    public enum ConfigSticks
    {
        [EnumMember(Value = "left_stick_x")]
        LeftStickX = 0,

        [EnumMember(Value = "left_stick_y")]
        LeftStickY = 1,

        [EnumMember(Value = "right_stick_x")]
        RightStickX = 2,

        [EnumMember(Value = "right_stick_y")]
        RightStickY = 3,
    }
}
