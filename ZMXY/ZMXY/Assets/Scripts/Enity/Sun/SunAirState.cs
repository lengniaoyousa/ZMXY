using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunAirState : SunState
{
    public SunAirState(SunController sun) : base(sun)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Execute()
    {
        base.Execute();
        
        if (Input.GetKeyDown(KeyCode.K) && jumpCount < 2)
        {
            Sun.stateMachine.ChangeState<SunDoubleJump>();
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}
