using Sirenix.OdinInspector;
using UnityEngine;

[System.Serializable]
public class SkillDamageConfig
{
    private bool mShowBox;//是否显示3Dbox碰撞体
    private bool mShowCircle;//是否显示3D圆球碰撞体
    
    [LabelText("触发时间")]
    public float triggerTime;

    [LabelText("结束时间")]
    public float EndTime;

    [LabelText("触发间隔")]
    public float TriggerInterval;

    [LabelText("是否跟随特效移动")]
    public bool IsFollowEffect;
    
    [LabelText("技能伤害配置")]
    public SkillDamageType damageType;

    [LabelText("伤害倍率")]
    public float damageRate;
    
    [LabelText("伤害检测方式"),OnValueChanged("OnDectectionValueChange")]
    public DamageDetectionMode damageDetectionMode;

    [LabelText("Box碰撞体宽高"),ShowIf("mShowBox")]
    public Vector3 BoxSize = new Vector3(1,1,1);
    
    [LabelText("Box碰撞体偏移"),ShowIf("mShowBox")]
    public Vector3 BoxOffset = new Vector3(0, 0, 0);
    
    [LabelText("圆球碰撞体偏移值"),ShowIf("mShowCircle")]
    public Vector3 SphereOffest = new Vector3(0,0,0);
    
    [LabelText("圆球伤害检测半径"),ShowIf("mShowCircle")]
    public float radius = 1;
    [LabelText("最小触发半径"),ShowIf("mShowCircle")]
    public float MinRadius ; // 最小触发半径
    [LabelText("最大触发半径"),ShowIf("mShowCircle")]
    public float MaxRadius ; // 最大触发半径

    [LabelText("扇形角度"),ShowIf("mShowCircle")]
    public float Angle;
    
    [LabelText("碰撞体位置类型")]
    public ColliderPosType colliderPosType ;
    
    [LabelText("伤害触发目标")]
    public LayerMask layerMask;

    [TitleGroup("附加Buff", "伤害触发时附加指定Buff")]
    public int[] AddBuffs;

    [TitleGroup("触发后续技能", "伤害触发后且技能释放完成后触发的技能")]
    public int triggerSkillId;
    
    /// <summary>
    /// 碰撞检测类型发生变化
    /// </summary>
    /// <param name="detectionMode"></param>
    public void OnDectectionValueChange(DamageDetectionMode detectionMode)
    {
        mShowBox = detectionMode == DamageDetectionMode.BOX3D;
        mShowCircle = detectionMode == DamageDetectionMode.Circle ;
    }
}

public enum SkillDamageType
{
    [LabelText("无伤害")]None,//无伤害
    [LabelText("物理伤害")] ADDamage,//物理伤害
    [LabelText("魔法伤害")] APDamage,//魔法伤害
}

public enum DamageDetectionMode
{
    [LabelText("无配置")] None,//无配置
    [LabelText("3DBox碰撞检测")] BOX3D,//Box碰撞检测
    [LabelText("3D圆球碰撞检测")] Circle,//圆球碰撞检测
    [LabelText("所有目标")] AllTarget,//通过代码搜索的所有目标
}

public enum ColliderPosType
{
    [LabelText("跟随角色朝向")] FollowDir,//跟随角色朝向
    [LabelText("跟随角色位置")] FollowPos,//跟随角色位置
    [LabelText("中心坐标")] ConterPos,//中心坐标
    [LabelText("目标位置")] TargetPos,//目标位置
}
