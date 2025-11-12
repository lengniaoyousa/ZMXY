#if UNITY_EDITOR
using UnityEditor;
using Sirenix.OdinInspector.Editor;
#endif
using UnityEngine;
using System.Collections.Generic;
using Sirenix.OdinInspector;

public class SkillComplierWindow : OdinEditorWindow
{
    /// <summary>
    /// 角色配置
    /// </summary>
    [TabGroup("SkillCharacter", "Character", SdfIconType.Robot, TextColor = "orange")]
    public SkillCharacterConfig SkillCharacterConfig;

    /// <summary>
    /// 技能基础配置
    /// </summary>
    [TabGroup("SKillComplier", "Skill", SdfIconType.Robot, TextColor = "lightmagenta")]
    public SkillConfig SkillConfig;

    [TabGroup("SKillComplier", "Buff", SdfIconType.Magic, TextColor = "red")]
    public List<SkillBuffConfig> buffList;


    [TabGroup("SKillComplier", "Effect", SdfIconType.OpticalAudio, TextColor = "blue")]
    public List<SkillEffectConfig> effectList;

    [TabGroup("SKillComplier", "Audio", SdfIconType.OpticalAudio, TextColor = "blue")]
    public List<SkillAudioConfig> audioList;


    [TabGroup("SKillComplier", "Action", SdfIconType.OpticalAudio, TextColor = "cyan")]
    public List<SkillActionConfig> actionList;

#if  UNITY_EDITOR
        #region 生命周期

    protected override void OnEnable()
    {
        base.OnEnable();
        
        EditorApplication.update += OnEditorUpdate;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        EditorApplication.update -= OnEditorUpdate;
    }

    #endregion
    
    /// <summary>
    /// 获取Editor模式下角色位置
    /// </summary>
    public static Vector3 GetCharacterPos()
    {
        if (!HasOpenInstances<SkillComplierWindow>())
        {
            return Vector3.zero;
        }
        
        SkillComplierWindow window = EditorWindow.GetWindow<SkillComplierWindow>();

        if (window.SkillCharacterConfig.SkillCharacter)
        {
            return window.SkillCharacterConfig.SkillCharacter.transform.position;
        }
        
        return Vector3.zero;
    }
    
    #region 展示和获取窗口

    [MenuItem("Skill/SkillComplierWindow")]
    public static SkillComplierWindow ShowWindow()
    {
        return GetWindowWithRect<SkillComplierWindow>(new Rect(0, 0, 1000, 600));
    }


    public static SkillComplierWindow GetWindow()
    {
        return GetWindow<SkillComplierWindow>() ; 
    }

    #endregion

    #region 加载和保存技能数据

    public void SaveSKillData()
    {
        SkillDataConfig.SaveSkillData(SkillCharacterConfig, SkillConfig,  effectList, audioList, actionList,buffList);
        Close();
    }
    /// <summary>
    /// 加载技能数据
    /// </summary>
    /// <param name="skillData"></param>
    public void LoadSkillData(SkillDataConfig skillData)
    {
        this.SkillCharacterConfig = skillData.character;
        this.SkillConfig = skillData.skillCfg;
        this.effectList = skillData.effectCfgList;
        this.audioList = skillData.audioCfgList;
        this.actionList = skillData.actionCfgList;
        this.buffList = skillData.buffCfgList;
    }

    #endregion
    
    #region 技能播放相关

    private float mAccLogicRuntime;//累计运行时间
    private float mNextLogicFrameTime;//执行下一次循环的时间
    private double mLastUpdateTime;//上次更新的时间
    //是否开始播放技能
    private bool isStartPlaySkill = false;

    /// <summary>
    /// 开始播放技能
    /// </summary>
    public void StartPlayAnim()
    {
        //创建技能特效
        foreach (var item in effectList)
        {
            item.StartPlaySkill();
        }
        
        mAccLogicRuntime = 0;
        mNextLogicFrameTime = 0;
        mLastUpdateTime = 0;
        isStartPlaySkill = true;
    }
    
    /// <summary>
    /// 技能暂停
    /// </summary>
    public void SkillPause()
    {
        //销毁特效
        foreach (var item in effectList)
        {
            item.PlaySkillEnd();
        }
    }

    public void PlaySkilEnd()
    {
        //调用特效结束接口
        foreach (var item in effectList)
        {
            item.PlaySkillEnd();
        }
        
        isStartPlaySkill = false;
        mAccLogicRuntime = 0;
        mNextLogicFrameTime = 0;
        mLastUpdateTime = 0;
    }

    #endregion
    
    #region 更新

    public void OnLogicUpdate()
    {
        if (mLastUpdateTime == 0)
        {
            mLastUpdateTime = EditorApplication.timeSinceStartup;
        }
        
        mAccLogicRuntime = (float)(EditorApplication.timeSinceStartup - mLastUpdateTime);
        while (mAccLogicRuntime>mNextLogicFrameTime)
        {
            OnLogicFrameUpdate();
            mNextLogicFrameTime += Time.fixedDeltaTime;
        }
    }

    public void OnLogicFrameUpdate()
    {
        //执行特效的OnLogicFrameUpdate
         foreach (var item in effectList)
         {
             item.OnLogicFrameUpdate();
         }
    }
    
    public void OnEditorUpdate()
    {
        try
        {
            SkillCharacterConfig.OnUpdate(()=> {
                //刷新当前窗口
                Focus();
            });
            if (isStartPlaySkill)
            {
                OnLogicUpdate();
            }
 
        }
        catch (System.Exception)
        {

        }
    }

    #endregion
#endif

    
}
