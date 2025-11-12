using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZM.ZMAsset;

public class Skill
{
    /// <summary>
    /// 技能id
    /// </summary>
    public int SkillId;
    
    /// <summary>
    ///技能创建者 
    /// </summary>
    public Enity SkillCreate;

    /// <summary>
    /// 技能配置数据
    /// </summary>
    private SkillDataConfig mSkillData;

    /// <summary>
    /// 技能基础配置
    /// </summary>
    public SkillConfig SkillCfg;

    /// <summary>
    /// 技能伤害列表
    /// </summary>
    public List<SkillDamageConfig> DamageCfgList;
    
    /// <summary>
    /// 技能特效配置
    /// </summary>
    private List<SkillEffectConfig> mEffectCfgList;
    
    /// <summary>
    /// 技能移动配置
    /// </summary>
    private List<SkillActionConfig> mActionCfgList;
    
    /// <summary>
    /// 技能音乐配置
    /// </summary>
    private List<SkillAudioConfig>  mAudioCfgList;

    /// <summary>
    /// 引导类型技能位置
    /// </summary>
    public Vector3 SkillGuidePos;

    /// <summary>
    /// 技能是否结束
    /// </summary>
    public bool IsSkillEnd;

    /// <summary>
    /// 组合技能id
    /// </summary>
    private int mCombinationSkillid;

    private float mCurrentRunTime = 0;

    public Skill(int skillId, Enity skillCreate)
    {
        SkillId = skillId;
        SkillCreate = skillCreate;
        mSkillData = ZMAsset.LoadScriptableObject<SkillDataConfig>(AssetPath.SKILL_DATA_PATH + skillId + ".asset");
        DamageCfgList = mSkillData.damageCfgList;
        mEffectCfgList = mSkillData.effectCfgList;
        mActionCfgList = mSkillData.actionCfgList;
    }

    /// <summary>
    /// 释放技能
    /// </summary>
    public void ReleaseSkill()
    {
        PlayAnim();

        SkillExecute();
    }

    /// <summary>
    /// 播放技能动画
    /// </summary>
    private void PlayAnim()
    {
        SkillCreate.animator.Play(mSkillData.character.SkillAnimClip.name);
    }


    /// <summary>
    /// 技能逻辑更新
    /// </summary>
    public void SkillExecute()
    {
        Debug.Log(mCurrentRunTime);
        
        CreateSkillEffect();
        
        if (mCurrentRunTime > mSkillData.character.AnimLength)
        {
            SkillEnd();
        }
        
        mCurrentRunTime += Time.deltaTime;
    }

    /// <summary>
    /// 创建技能特效
    /// </summary>
    private async void CreateSkillEffect()
    {
        foreach (SkillEffectConfig skillEffectConfig in mSkillData.effectCfgList)
        {
            GameObject effectObj = null;
            if (!skillEffectConfig.mEffectCreated && mCurrentRunTime >= skillEffectConfig.TriggerTime)
            {
                Debug.Log("创建特效");
                skillEffectConfig.mEffectCreated = true;
                AssetsRequest assetsRequest = await ZMAsset.InstantiateObjectAsync(skillEffectConfig.SkillEffectPath,null);

                effectObj = assetsRequest.obj;
                
                effectObj.transform.position = SkillCreate.transform.position;

                effectObj.transform.position =  new Vector3(effectObj.transform.position.x + skillEffectConfig.effectOffsetPos.x * SkillCreate.GetMianChaoXiang(),
                    effectObj.transform.position.y + skillEffectConfig.effectOffsetPos.y , effectObj.transform.position.z + skillEffectConfig.effectOffsetPos.z);
                
                effectObj.transform.localScale = skillEffectConfig.EffectScale;
            }
        }
        
    }

    /// <summary>
    /// 销毁技能特效
    /// </summary>
    private void DestorySkillEffect()
    {
        
    }


    private void SkillEnd()
    {
        foreach (var skillEffect in mEffectCfgList)
        {
            skillEffect.mEffectCreated = false;
        }
        
        IsSkillEnd = true;
    }
}
