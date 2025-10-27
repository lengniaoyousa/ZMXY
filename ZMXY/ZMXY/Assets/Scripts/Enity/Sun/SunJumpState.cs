using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunJumpState : SunAirState
{
    private static readonly int Jump = Animator.StringToHash("Jump");

    private float jumpTimer = 0.4f;

    private float JumpFore = 4;

    private float JumpScale = 1;


    public SunJumpState(SunController sun) : base(sun)
    {

    }

    public override void Enter()
    {
        base.Enter();
        Sun.animator.SetBool(Jump, true);
        Debug.Log("SunJumpState Enter");
        jumpTimer = 0.4f;
        JumpFore = 6;
        JumpScale = 1;
        jumpCount++;
    }

    public override void Execute()
    {
        base.Execute();
        if (Input.GetKey(KeyCode.K))
        {
            jumpTimer -= Time.deltaTime;
            Sun.rigidbody2D.velocity = new Vector2(Sun.airSpeed *Sun.GetInputX() , JumpFore*JumpScale);
            Debug.Log(Sun.GetMianChaoXiang());
            
            JumpScale += Time.deltaTime;
            
            if (jumpTimer<0)
            {
                Sun.stateMachine.ChangeState<SunFallState>();
            }
        }
        
        if (Input.GetKeyUp(KeyCode.K))
        {
            Sun.stateMachine.ChangeState<SunFallState>();
        }
    }

    public override void Exit()
    {
        base.Exit();
        Sun.animator.SetBool(Jump, false);
        Debug.Log("SunJumpState Exit");
    }
}
