using System;

public abstract class ActionBehaviour  
{
    /// <summary>
    /// 是否移动完成
    /// </summary>
    public bool actionFinsih = false;
    /// <summary>
    /// 移动完成回调
    /// </summary>
    protected Action mActionFinishCalllBack;
    /// <summary>
    /// 更新行动回调
    /// </summary>
    protected Action mUpdateActionCallBack;
    /// <summary>
    /// 逻辑帧更新
    /// </summary>
    public abstract void OnUpdate();
    /// <summary>
    /// 行动完成
    /// </summary>
    public abstract void OnActionFinish();
}