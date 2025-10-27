using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunFallState : SunAirState
{
    private static readonly int Fall = Animator.StringToHash("Fall");

    public SunFallState(SunController sun) : base(sun)
    {
    }

    public override void Enter()
    {
        base.Enter();
        Sun.animator.SetBool(Fall,true);
    }

    public override void Execute()
    {
        base.Execute();
        
        //空中移动速度
        Sun.rigidbody2D.velocity = new Vector2(Sun.airSpeed *Sun.GetInputX() , Sun.rigidbody2D.velocity.y);

        if (Sun.IsGrounded())
        {
            Sun.stateMachine.ChangeState<SunIdleState>();
        }
    }

    public override void Exit()
    {
        base.Exit();
        Sun.animator.SetBool(Fall,false);
    }
}
