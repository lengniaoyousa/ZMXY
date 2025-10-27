using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TimerBehaviour
{
    /// <summary>
    /// 计时是否完成
    /// </summary>
    public bool TimerFinsih = false;
    /// <summary>
    /// 计时完成回调
    /// </summary>
    protected Action mTimerFinishCalllBack;
    /// <summary>
    /// 计时行动回调
    /// </summary>
    protected Action mUpdateTiemrCallBack;
    /// <summary>
    /// 逻辑帧更新
    /// </summary>
    public abstract void OnLogicFrameUpdate();
    /// <summary>
    /// 行动完成
    /// </summary>
    public abstract void OnTimerFinish();
}