using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunDoubleJump:SunAirState
{
    private static readonly int DoubleJump = Animator.StringToHash("DoubleJump");
    private float jumpTimer = 0.3f;

    private float JumpFore = 3;

    private float JumpScale = 1;
    
    public SunDoubleJump(SunController sun) : base(sun)
    {
    }

    public override void Enter()
    {
        base.Enter();
        Sun.animator.SetBool(DoubleJump,true);
        jumpTimer = 0.3f;
        JumpFore = 5;
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
        Sun.animator.SetBool(DoubleJump,false);
    }
}
