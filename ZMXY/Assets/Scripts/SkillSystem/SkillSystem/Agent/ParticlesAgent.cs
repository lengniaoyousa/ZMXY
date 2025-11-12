using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ParticlesAgent 
{
#if UNITY_EDITOR

    private ParticleSystem[] mParticleArr;
    private double mLastRunTime;
    private double curRunTime;
    
    public void InitPlayAnim(Transform trans)
    {
        mParticleArr = trans.GetComponentsInChildren<ParticleSystem>();
        EditorApplication.update += OnUpdate;
    }

    public void OnDestroy()
    {
        EditorApplication.update -= OnUpdate;
    }
    public void OnUpdate()
    {
        if (mLastRunTime == 0)
        {
            mLastRunTime = EditorApplication.timeSinceStartup;
        }
        //当前运行时间
        curRunTime = EditorApplication.timeSinceStartup - mLastRunTime;

        if (mParticleArr != null)
        {
            foreach (var item in mParticleArr)
            {
                if (item != null)
                {
                    //清理粒子
                    item.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
                    //使用固定的随机种子每次播放完全一致
                    item.useAutoRandomSeed = false;
                    //模拟指定粒子更新时间
                    item.Simulate((float)curRunTime);
                }
            }
        }
    }
#endif
}
