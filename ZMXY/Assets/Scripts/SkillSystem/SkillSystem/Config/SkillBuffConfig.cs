using Sirenix.OdinInspector;
using System.Collections.Generic;
[System.Serializable]
public class SkillBuffConfig 
{
    [LabelText("Buff类型")]
    public BuffType buffType;

    [LabelText("buff持续时间"), GUIColor("green")]
    public float buffContinuedTime;
    [LabelText("Buff 数值配置")]
    public List<BuffParams> buffParamsList;
}
public enum BuffType
{
    [LabelText("无配置")]
    None,
    [LabelText("根据角色位置击退")]
    PositionRepelled,
    [LabelText("根据角色面朝向击退")]
    DirRepelled

}
[System.Serializable]
public class BuffParams
{
    [LabelText("参数"), PropertyTooltip("例如：造成的伤害量，击退的距离")]
    public float value;
    [LabelText("参数描述")]
    public string des;
}