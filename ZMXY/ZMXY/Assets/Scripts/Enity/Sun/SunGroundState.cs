using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunGroundState:SunState
{
    public SunGroundState(SunController sun) : base(sun)
    {
        
    }
    
    #region 双击检测

    [Header("双击检测设置")]
    
    public bool isRunning = false;
    
    public float doubleClickATime = 0.5f; // 双击时间间隔
    public KeyCode AtKey = KeyCode.A; // 目标键位
    
    protected int clickACount = 0;
    protected float lastClickATime = 0f;
    
    public float doubleClickBTime = 0.5f; // 双击时间间隔
    public KeyCode BtKey = KeyCode.D; // 目标键位
    
    protected int clickBCount = 0;
    private float lastClickBTime = 0f;

    void DetectDoubleClick()
    {
        if (Sun.IsGrounded())
        {
            if (Input.GetKeyDown(AtKey))
            {
                clickACount++;
            
                if (clickACount == 1)
                {
                    lastClickATime = Time.time;
                }
            
                if (clickACount > 1 && Time.time - lastClickATime < doubleClickATime)
                {
                    //双击成功
                    isRunning = true;
                    clickACount = 0;
                }
                else if (Time.time - lastClickATime >= doubleClickATime)
                {
                    // 超时，重置计数
                    isRunning = false;
                    clickACount = 1;
                    lastClickATime = Time.time;
                }
            }

            if (Input.GetKeyDown(BtKey))
            {
                clickBCount++;

                if (clickBCount == 1)
                {
                    lastClickBTime = Time.time;
                }

                if (clickBCount>1&&Time.time - lastClickBTime < doubleClickBTime)
                {
                    isRunning = true;
                    clickBCount = 0;
                }
                else if (Time.time - lastClickBTime >= doubleClickBTime)
                {
                    isRunning = false;
                    clickBCount = 1;
                    lastClickBTime = Time.time;
                }
            }

            if (Input.GetKeyDown(KeyCode.K))
            {
                Sun.stateMachine.ChangeState<SunJumpState>();
            }
        }
        
    }

    #endregion

    public override void Enter()
    {
        base.Enter();
    }

    public override void Execute()
    {
        base.Execute();
        DetectDoubleClick();

        if (Input.GetKeyDown(KeyCode.J))
        {
            Sun.stateMachine.ChangeState<SunAttackState>();
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}
