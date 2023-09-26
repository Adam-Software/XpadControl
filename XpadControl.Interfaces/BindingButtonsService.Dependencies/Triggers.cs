using System.Runtime.Serialization;

namespace XpadControl.Interfaces.BindingButtonsService.Dependencies
{
    public enum Triggers
    {
        [EnumMember(Value = "left_trigger")]
        LeftTrigger = 0,

        [EnumMember(Value = "right_trigger")]
        RightTrigger = 1
    }
}
