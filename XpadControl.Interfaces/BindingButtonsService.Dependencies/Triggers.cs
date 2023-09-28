using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace XpadControl.Interfaces.BindingButtonsService.Dependencies
{
    [JsonConverter(typeof(JsonStringEnumMemberConverter))]
    public enum Triggers
    {
        [EnumMember(Value = "left_trigger")]
        LeftTrigger = 0,

        [EnumMember(Value = "right_trigger")]
        RightTrigger = 1
    }
}
