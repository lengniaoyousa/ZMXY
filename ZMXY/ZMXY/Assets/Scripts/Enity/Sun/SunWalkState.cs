using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunWalkState : SunGroundState
{
    private static readonly int Walk = Animator.StringToHash("Walk");
    
    public SunWalkState(SunController sun) : base(sun)
    {
        
    }

    public override void Enter()
    {
        base.Enter();
        
        Sun.animator.SetBool(Walk, true);
        Debug.Log("SunWalkState Enter");
    }

    public override void Execute()
    {
        base.Execute();
        if (Input.GetKey(KeyCode.A)||Input.GetKey(KeyCode.D))
        {
            Sun.rigidbody2D.velocity = new Vector2(Sun.moveSpeed * Sun.GetMianChaoXiang(), Sun.rigidbody2D.velocity.y);
        }
        else if (Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.D))
        {
            Sun.stateMachine.ChangeState<SunIdleState>();
        }
    }

    public override void Exit()
    {
        base.Exit();
        Sun.animator.SetBool(Walk, false);
    }
}