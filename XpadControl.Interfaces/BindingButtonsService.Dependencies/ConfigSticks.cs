using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace XpadControl.Interfaces.BindingButtonsService.Dependencies
{
    [JsonConverter(typeof(JsonStringEnumMemberConverter))]
    public enum ConfigSticks
    {
        [EnumMember(Value = "none")]
        None = 0,

        [EnumMember(Value = "left.stick.x")]
        LeftStickX = 1,

        [EnumMember(Value = "left.stick.y")]
        LeftStickY = 2,

        [EnumMember(Value = "right.stick.x")]
        RightStickX = 3,

        [EnumMember(Value = "right.stick.y")]
        RightStickY = 4,
    }
}
