using Sirenix.OdinInspector;
using UnityEngine;

[System.Serializable]
public class SkillEffectConfig
{
    [HideInInspector] public GameObject EffectObj;

    private bool mEffectCreated = false;

    //Editor模式下克隆的特效对象
    private GameObject mCloneEffect;
    private AnimationAgnet mAnimAgent;

    private ParticlesAgent mParticleAgent;

    //当前运行时间
    private float mCurRunTime = 0;

    [AssetList]
    [LabelText("技能特效"), TitleGroup("角色动画特效", "跟随角色动作生成的特效，想要实现脱手技能请使用Buff系统")]
    [PreviewField(70, ObjectFieldAlignment.Left), OnValueChanged("GetSkillEffectPath")]
    public GameObject SkillEffect;

    [ReadOnly] [LabelText("技能特效路径")] public string SkillEffectPath;

    [LabelText("特效动画"), OnValueChanged("GetAnimLength")]
    public AnimationClip SkillEffectAnim;

    [LabelText("特效动画时长")] [ReadOnly] public float SkillEffectAnimLength;

    [LabelText("触发时间")] public float TriggerTime;

    [LabelText("特效结束时间")] public float EndTime;

    [LabelText("特效缩放")] public Vector3 EffectScale;

    [LabelText("特效偏移位置")] public Vector3 effectOffsetPos;

    [LabelText("特效旋转")] public Vector3 EffectRotation;

    [LabelText("角色面朝向")] public int characterFilp = -1;

    [LabelText("是否设置特效父节点")] public bool isSetTransParent = false;

    [LabelText("父节点"), ShowIf("isSetTransParent")]
    public TransParentType transParent;

    [LabelText("是否附加伤害")] public bool IsAddDamage;

    [LabelText("伤害配置"), ShowIf("IsAddDamage")]
    public SkillDamageConfig skillDamageCfg;

    [LabelText("是否跟随玩家移动")] public bool IsFollowRelease;
    [LabelText("是否是移动特效")] public bool IsMoveEffect;

    [LabelText("特效移动"), ShowIf("IsMoveEffect")]
    public SkillActionConfig ActionConfig;


    #region 特效配置字段监听函数

    public void GetSkillEffectPath()
    {
        SkillEffectPath = UnityEditor.AssetDatabase.GetAssetPath(SkillEffect);
    }

    public void GetAnimLength()
    {
        if (SkillEffectAnim)
        {
            SkillEffectAnimLength = SkillEffectAnim.length;
        }
    }

    #endregion

    #region 特效动画播放流程

    /// <summary>
    /// 开始播放技能特效
    /// </summary>
    public void StartPlaySkill()
    {
        mEffectCreated = false;
        DestroyEffect();
        mCurRunTime = 0;
    }

    public void SkillPause()
    {
        DestroyEffect();
    }

    public void PlaySkillEnd()
    {
        DestroyEffect();
    }

    #endregion

    public void OnLogicFrameUpdate()
    {
        if (!mEffectCreated && mCurRunTime >= TriggerTime)
        {
            CreateEffect();
            mEffectCreated = true;
        }
        else if (mCurRunTime >= EndTime)
        {
            DestroyEffect();
        }

        mCurRunTime += Time.fixedDeltaTime;
    }


    #region 创建和销毁特效

    public void CreateEffect()
    {
        if (SkillEffect)
        {
            mCloneEffect = GameObject.Instantiate(SkillEffect);

            mCloneEffect.transform.rotation = Quaternion.Euler(EffectRotation);

            mCloneEffect.transform.localScale = EffectScale;

            //角色位置加特效偏移位置
            mCloneEffect.transform.position = SkillComplierWindow.GetCharacterPos() +
                                              new Vector3(characterFilp * effectOffsetPos.x, effectOffsetPos.y,
                                                  effectOffsetPos.z);

            if (skillDamageCfg.damageDetectionMode == DamageDetectionMode.BOX3D)
            {
                DrawBoxColliderGizmo boxCollider = mCloneEffect.GetComponent<DrawBoxColliderGizmo>();

                boxCollider.size = skillDamageCfg.BoxSize;

                boxCollider.center = skillDamageCfg.BoxOffset;

                //boxCollider.rotation =  mCloneEffect.transform.rotation;

                //Vector2 size = boxCollider.size;
            }
            else if (skillDamageCfg.damageDetectionMode == DamageDetectionMode.Circle)
            {
                DrawCircleColliderGizmo circleCollider = mCloneEffect.GetComponent<DrawCircleColliderGizmo>();
                circleCollider.maxRadius = skillDamageCfg.radius;
                circleCollider.angle = skillDamageCfg.Angle;
            }

            mAnimAgent = new AnimationAgnet();
            mAnimAgent.InitPlayAnim(mCloneEffect.transform, SkillEffectAnim);

            mParticleAgent = new ParticlesAgent();
            mParticleAgent.InitPlayAnim(mCloneEffect.transform);
        }
    }

    public void DestroyEffect()
    {
        if (mCloneEffect)
        {
            GameObject.DestroyImmediate(mCloneEffect);
        }

        if (mAnimAgent != null)
        {
            mAnimAgent.OnDestroy();
            mAnimAgent = null;
        }

        if (mParticleAgent != null)
        {
            mParticleAgent.OnDestroy();
            mParticleAgent = null;
        }
    }

    public void ResetEffect()
    {
        mEffectCreated = false;
        mCurRunTime = 0f;
        DestroyEffect();
    }

    #endregion
}

public enum TransParentType
{
    [LabelText("无配置")] None,
}