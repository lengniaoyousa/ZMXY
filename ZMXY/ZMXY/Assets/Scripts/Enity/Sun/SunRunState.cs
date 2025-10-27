using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunRunState:SunGroundState
{
    private static readonly int Run = Animator.StringToHash("Run");

    public SunRunState(SunController sun) : base(sun)
    {
        
    }

    public override void Enter()
    {
        base.Enter();
        Debug.Log("SunRunState:Enter");
        Sun.animator.SetBool(Run,true);
    }

    public override void Execute()
    {
        base.Execute();
        Sun.rigidbody2D.velocity = new Vector2((Sun.runSpeed) * Sun.GetMianChaoXiang(), Sun.rigidbody2D.velocity.y);
        
        if (Sun.GetInputX() == 0)
        {
            Sun.stateMachine.ChangeState<SunIdleState>();
        }
    }

    public override void Exit()
    {
        Debug.Log("SunRunState:Exit");
        isRunning = false;
        clickACount = 0;
        clickBCount = 0;
        Sun.animator.SetBool(Run,false);
        base.Exit();
    }
}
