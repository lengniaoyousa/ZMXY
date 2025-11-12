using System;
using UnityEngine;

/// <summary>
/// 移动类型
/// </summary>
public enum MoveType
{
    target,
    X,
    Y,
    Z,
}
public class MoveToAction : ActionBehaviour
{
    /// <summary>
    /// 移动对象
    /// </summary>
    private GameObject mActionObj;
    
    /// <summary>
    /// 开始移动位置
    /// </summary>
    private Vector3 mStartPos;
    
    /// <summary>
    /// 移动所需时间
    /// </summary>
    private float mMoveTime;
    
    /// <summary>
    /// 移动类型
    /// </summary>
    private MoveType mMoveType;
    
    /// <summary>
    /// 移动的向量
    /// </summary>
    private Vector3 mMoveDistance;
    
    /// <summary>
    /// 当前累计运行的时间
    /// </summary>
    private int mAccRumTime; 
    
    /// <summary>
    /// 当前移动的时间缩放
    /// </summary>
    private float mTimeScale;

    public MoveToAction(GameObject actionObj, Vector3 startPos, Vector3 targetPos, float moveTime, Action moveFinishCallBack,
        Action updateCallBack, MoveType moveType)
    {
        mActionObj = actionObj;
        mStartPos = startPos;
        mMoveTime = moveTime;
        mMoveType = moveType;
        mActionFinishCalllBack = moveFinishCallBack;
        mUpdateActionCallBack = updateCallBack;
        //目标位置减去起始位置等于移动向量
        mMoveDistance = targetPos - startPos;
    }
    
    public override void OnUpdate()
    {
        mAccRumTime+=20;
        
        mTimeScale = mAccRumTime/mMoveTime;

        if (mTimeScale>=1)
        {
            mTimeScale = 1;
            actionFinsih = true;
        }
        
        mUpdateActionCallBack?.Invoke();
        
        Vector3 addDistance = Vector3.zero;

        if (mMoveType == MoveType.target)
        {
            addDistance = mMoveDistance * mTimeScale;
            mActionObj.transform.position = mStartPos + addDistance;
        }
        else if(mMoveType == MoveType.X)
        {
            addDistance.x = mMoveDistance.x * mTimeScale;
            mActionObj.transform.position = new Vector3(mStartPos.x + addDistance.x,mStartPos.y,mStartPos.z);
        }
        else if(mMoveType == MoveType.Y)
        {
            addDistance.y = mMoveDistance.y * mTimeScale;
            mActionObj.transform.position = new Vector3(mStartPos.x ,mStartPos.y + addDistance.y,mStartPos.z);
        }
        else if(mMoveType == MoveType.Z)
        {
            addDistance.z = mMoveDistance.z * mTimeScale;
            mActionObj.transform.position = new Vector3(mStartPos.x ,mStartPos.y ,mStartPos.z + addDistance.z);
        }
        
    }

    public override void OnActionFinish()
    {
        if (actionFinsih)
        {
            mActionFinishCalllBack?.Invoke();
        }
    }
}
