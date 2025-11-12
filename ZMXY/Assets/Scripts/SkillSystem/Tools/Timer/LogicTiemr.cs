using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LogicTiemr : TimerBehaviour
{

    private int mDelayTime;
    private int mLoopCount;

    private int mCurLogicFrameAccTime;
    /// <summary>
    /// 总运行时间
    /// </summary>
    private float mTotalTime;
    public LogicTiemr(int delayTime, Action updateAction, Action timerCallBack, int loopCount = 1) //1,2
    {
        this.mDelayTime = delayTime;
        this.mLoopCount = loopCount;
        this.mTotalTime = loopCount * delayTime;
        this.mTimerFinishCalllBack = timerCallBack;
        this.mUpdateTiemrCallBack = updateAction;
    }
    /// <summary>
    /// 逻辑帧更新
    /// </summary>
    public override void OnLogicFrameUpdate()
    {
        mUpdateTiemrCallBack?.Invoke();
        mCurLogicFrameAccTime += 20;
        if (mCurLogicFrameAccTime >= mDelayTime)
        {
            //Debug.Log($"mCurLogicFrameAccTime:{mCurLogicFrameAccTime}   mDelayTime:{mDelayTime}");
            mTimerFinishCalllBack?.Invoke();
            mCurLogicFrameAccTime -= mDelayTime;
            mTotalTime -= mDelayTime;
            //如果循环次数<=1 说明当前计时器工作完成
            if (mLoopCount <= 1 || mTotalTime <= 0)
            {
                TimerFinsih = true;
                mTimerFinishCalllBack = null;
                mUpdateTiemrCallBack = null;
            }
        }

    }

    public override void OnTimerFinish()
    {

    }
}