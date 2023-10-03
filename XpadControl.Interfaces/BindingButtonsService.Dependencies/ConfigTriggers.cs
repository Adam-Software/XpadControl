using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace XpadControl.Interfaces.BindingButtonsService.Dependencies
{
    [JsonConverter(typeof(JsonStringEnumMemberConverter))]
    public enum ConfigTriggers
    {
        [EnumMember(Value = "none")]
        None = 0,

        [EnumMember(Value = "left.trigger")]
        LeftTrigger = 1,

        [EnumMember(Value = "right.trigger")]
        RightTrigger = 2
    }
}
