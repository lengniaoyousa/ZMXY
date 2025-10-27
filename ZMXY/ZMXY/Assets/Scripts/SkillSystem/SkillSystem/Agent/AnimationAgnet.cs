using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
public class AnimationAgnet
{
#if UNITY_EDITOR
    private AnimationClip mAnimClip;
    private double mLastRunTime;
    private GameObject gameObject;
    public void InitPlayAnim(Transform trans, AnimationClip animationClip)
    {
        mAnimClip = animationClip;
        gameObject = trans.gameObject;
        EditorApplication.update += OnUpdate;
    }

    public void OnDestroy()
    {
        EditorApplication.update -= OnUpdate;
    }
    public void OnUpdate()
    {
        if (mAnimClip != null)
        {
            if (mLastRunTime == 0)
            {
                mLastRunTime = EditorApplication.timeSinceStartup;
            }
            //当前运行时间
            double curRunTime = EditorApplication.timeSinceStartup - mLastRunTime;

            //动画播放比例
            //float curAnimNormalizationValue = (float)curRunTime / mAnimClip.length;
            //动画采样
            if(gameObject!=null)
            {
                mAnimClip.SampleAnimation(gameObject, (float)curRunTime);
            }
            
        }

    }
#endif
}
