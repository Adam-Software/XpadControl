using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace XpadControl.Interfaces.WebSocketClientsService.Dependencies
{
    /// <summary>
    /// Actual version name and id <see cref="https://raw.githubusercontent.com/Adam-Software/Adam-SDK/main/examples/servo_range.config"/>
    /// The enumeration constants correspond to the id of the servomotor
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumMemberConverter))]
    public enum ServoNames
    {
        [EnumMember(Value = "head")]
        Head = 1,

        [EnumMember(Value = "neck")]
        Neck = 2,

        [EnumMember(Value = "right_hand")]
        RightHand = 3,

        [EnumMember(Value = "left_hand")]
        LeftHand = 4,

        [EnumMember(Value = "right_lower_arm_up")]
        RightLowerArmUp = 7,

        [EnumMember(Value = "left_lower_arm_up")]
        LeftLowerArmUp = 8,

        [EnumMember(Value = "right_upper_arm")]
        RightUpperArm = 9,

        [EnumMember(Value = "left_upper_arm")]
        LeftUpperArm = 10,

        [EnumMember(Value = "right_shoulder")]
        RightShoulder = 11,

        [EnumMember(Value = "left_shoulder")]
        LeftShoulder = 12,

        [EnumMember(Value = "chest")]
        Chest = 13,

        [EnumMember(Value = "press")]
        Press = 14,

        [EnumMember(Value = "left_upper_leg")]
        LeftUpperLeg = 16,

        [EnumMember(Value = "right_upper_leg")]
        RightUpperLeg = 17,

        [EnumMember(Value = "left_lower_leg")]
        LeftLowerLeg = 18,

        [EnumMember(Value = "right_lower_leg")]
        RightLowerLeg = 19,

        [EnumMember(Value = "left_foot")]
        LeftFoot = 20,

        [EnumMember(Value = "right_foot")]
        RightFoot = 21
    }
}
