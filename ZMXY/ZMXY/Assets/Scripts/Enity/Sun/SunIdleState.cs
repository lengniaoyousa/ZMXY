using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunIdleState : SunGroundState
{
    private static readonly int Idle = Animator.StringToHash("Idle");

    public SunIdleState(SunController sun) : base(sun)
    {
        
    }

    public override void Enter()
    {
        base.Enter();
        Sun.animator.SetBool(Idle,true);
        isRunning = false;
        jumpCount = 0;
    }

    public override void Execute()
    {
        base.Execute();
        
        if (isRunning)
        {
            Sun.stateMachine.ChangeState<SunRunState>();
        }
        else
        {
            if (Sun.GetInputX() != 0)
            {
                Sun.stateMachine.ChangeState<SunWalkState>();
            }
        }
    }

    public override void Exit()
    {
        base.Exit();
        Sun.animator.SetBool(Idle,false);
    }
}
