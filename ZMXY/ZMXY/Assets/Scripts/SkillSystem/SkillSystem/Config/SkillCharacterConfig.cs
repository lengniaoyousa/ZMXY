#if UNITY_EDITOR
using UnityEditor;
using Sirenix.OdinInspector.Editor;
#endif
using UnityEngine;
using System.Collections.Generic;
using Sirenix.OdinInspector;

[HideMonoScript]
[System.Serializable]
public class SkillCharacterConfig
{
    private GameObject mTempCharacter;
    
    private Animator mAnimator;

    private double mLastRunTime;

    private bool mIsPlayAnim;
    
    [AssetList]
    [LabelText("角色预制体")]
    [PreviewField(70,ObjectFieldAlignment.Center)]
    public GameObject SkillCharacter;
    
    [TitleGroup("技能播放动画","释放技能时所播放的动画")]
    [LabelText("技能动画")]
    public AnimationClip SkillAnimClip;

    [BoxGroup("动画数据")]
    [HideLabel]
    [ProgressBar(0,100,r:0,g:255,b:0,Height = 30)]
    [OnValueChanged("OnAnimProgressValueChange")]
    public float AnimProgress;
    
    [BoxGroup("动画数据")]
    [LabelText("是否是循环动画")]
    public bool IsLoopAnim = false;
    
    [BoxGroup("动画数据")]
    [LabelText("动画循环次数")]
    [ShowIf("IsLoopAnim")]
    public int AnimLoopCount;
    
    [BoxGroup("动画数据")]
    [LabelText("是否自定义动画长度")]
    public bool IsSetCustomLength;
    
    [BoxGroup("动画数据")]
    [LabelText("自定义动画长度")]
    [ShowIf("IsSetCustomLength")]
    public int CustomLength = 0;

    [BoxGroup("动画数据")]
    [LabelText("动画时长")]
    [ReadOnly]
    public float AnimLength = 0;
    
    [BoxGroup("动画数据")]
    [LabelText("当前动画时长")]
    [ReadOnly]
    public float CurrentAnimLength = 0;

    #region 按钮相关

    [GUIColor(0.4f,0.8f,1)]
    [ButtonGroup("按钮数组")]
    [Button("播放",ButtonSizes.Large)]
    public void Play()
    {
        if (SkillCharacter)
        {
            string characterName = SkillCharacter.name;
            mTempCharacter = GameObject.Find(characterName);
            if (!mTempCharacter)
            {
                mTempCharacter = GameObject.Instantiate(SkillCharacter);
                mTempCharacter.name = mTempCharacter.name.Replace("(Clone)", "");
            }

            mAnimator = mTempCharacter.GetComponentInChildren<Animator>();
            
            AnimLength = IsLoopAnim? SkillAnimClip.length*AnimLoopCount :SkillAnimClip.length;

            mLastRunTime = 0;

            mIsPlayAnim = true;

            SkillComplierWindow window = SkillComplierWindow.GetWindow();
            
            window?.StartPlayAnim();

        }
    }
    
    [ButtonGroup("按钮数组")]
    [Button("暂停",ButtonSizes.Large)]
    public void Pause()
    {
        mIsPlayAnim = false;
        SkillComplierWindow window = SkillComplierWindow.GetWindow();
        window?.SkillPause();
    }
    
    [GUIColor(0, 1, 0)]
    [ButtonGroup("按钮数组")]
    [Button("保存配置", ButtonSizes.Large)]
    public void SaveAssets()
    {
        SkillComplierWindow.GetWindow().SaveSKillData();
    }

    #endregion
    
    /// <summary>
    /// 拖动动画进度条
    /// </summary>
    /// <param name="value"></param>
    public void OnAnimProgressValueChange(float value)
    {
        string characterName = SkillCharacter.name;
        mTempCharacter = GameObject.Find(characterName);

        if (!mTempCharacter)
        {
            mTempCharacter = GameObject.Instantiate(SkillCharacter);
            mTempCharacter.name = mTempCharacter.name.Replace("(Clone)","");
        }
        mAnimator = mTempCharacter.GetComponentInChildren<Animator>();
        
        float progressValue = (value/100)*SkillAnimClip.length;
        
        CurrentAnimLength = progressValue;
        
        SkillAnimClip.SampleAnimation(mTempCharacter.transform.GetChild(0).gameObject,progressValue);
        
    }
    
    
    public void OnUpdate(System.Action callback)
    {
        if (mIsPlayAnim)
        {
            if (mLastRunTime == 0)
            {
                mLastRunTime = EditorApplication.timeSinceStartup;
            }
            
            //获取当前运行时间
            double curRunTime = EditorApplication.timeSinceStartup - mLastRunTime;

            //计算动画播放进度
            float curAnimNormalizationValue = (float)curRunTime / AnimLength;

            AnimProgress = Mathf.Clamp(curAnimNormalizationValue * 100, 0, 100);
            
            float progressValue = AnimProgress * SkillAnimClip.length;
        
            CurrentAnimLength = progressValue/100;
            
            //动画采样
            SkillAnimClip.SampleAnimation(mTempCharacter.transform.GetChild(0).gameObject,(float)curRunTime);

            //角色动画播放结束
            if (Mathf.Approximately(AnimProgress, 100))
            {
                PlaySkillEnd();
            }
            
            callback?.Invoke();
        }
        
    }
    
    
    public void PlaySkillEnd()
    {
        mIsPlayAnim = false;
    
        SkillComplierWindow window = SkillComplierWindow.GetWindow();
        window?.PlaySkilEnd();
    }
}
