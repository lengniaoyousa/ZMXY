using Sirenix.OdinInspector;
using UnityEngine;

[HideMonoScript]          
[System.Serializable]     
public class SkillConfig
{
    [LabelText("技能图标"),LabelWidth(0.1f),PreviewField(70,ObjectFieldAlignment.Left),SuffixLabel("技能图标")]
    public Sprite SkillIcon;

    [LabelText("技能名称")]
    public string SkillName;

    [LabelText("技能前摇")]
    public float skillBefore;

    [LabelText("技能持续时间")]
    public float skillRelease;
 
    [LabelText("技能后摇")]
    public float skillAfter;

    [LabelText("技能所需蓝量")]
    public int NeedEnergyValue;

    [LabelText("技能冷却时间")]
    public int SkillCdTime;
 
    [LabelText("技能类型"),OnValueChanged("OnSkillTypeChange")]
    public SkillType SkillType;
 
    [LabelText("技能命中特效"),TitleGroup("技能渲染","释放技能时触发"),OnValueChanged("GetObjectPath")]
    public GameObject SkillHitEffect;

    [ReadOnly]
    public string SkillHitEffectPath;

    [LabelText("技能命中特效存活时间"), TitleGroup("技能渲染", "释放技能时触发")]
    public float HitEffectSurvivalTime;
 
    [LabelText("技能命中音效"), TitleGroup("技能渲染", "释放技能时触发")]
    public AudioClip SkillHitSound;

    [LabelText("是否显示技能立绘"), TitleGroup("技能渲染", "释放技能时触发")]
    public bool ShowSkillPortrait;
 
    [LabelText("技能立绘"),TitleGroup("技能渲染", "释放技能时触发"),ShowIf("ShowSkillPortrait")]
    public GameObject SkillPortraitObj;

    [LabelText("技能描述"), TitleGroup("技能渲染", "释放技能时触发")]
    public string SkillDes;

    public void OnSkillTypeChange(SkillType sKillType)
    {
    
    }
 
    public void GetObjectPath()
    {
        SkillHitEffectPath = UnityEditor.AssetDatabase.GetAssetPath(SkillHitEffect); 
    }
}
public enum SkillType
{
    [LabelText("无配置(瞬发技能)")]
    None,
    [LabelText("蓄力技能")]
    StockPile,
    [LabelText("位置引导技能")]
    PosGuide,
}
