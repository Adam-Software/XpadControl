using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace XpadControl.Interfaces.BindingButtonsService.Dependencies
{
    [JsonConverter(typeof(JsonStringEnumMemberConverter))]
    public enum AdamActions
    {
        [EnumMember(Value = "none")]
        None = 0,

        #region Riding

        [EnumMember(Value = "riding_left_right")]
        RidingLeftRight = 1,

        [EnumMember(Value = "riding_forward_backward")]
        RidingForwardBackward = 2,

        [EnumMember(Value = "turn_to_left")]
        TurnToLeft = 3,

        [EnumMember(Value = "turn_to_right")]
        TurnToRight = 4,

        #endregion

        #region Servo

        [EnumMember(Value = "to_home_position")]
        ToHomePosition = 5,

        [EnumMember(Value = "head_left_right")]
        HeadLeftRight = 6,

        [EnumMember(Value = "neck_up_down")]
        NeckUpDown = 7

        #endregion


    }
}
