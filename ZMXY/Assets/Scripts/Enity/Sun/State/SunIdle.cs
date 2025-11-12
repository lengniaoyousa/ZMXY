using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class SunController
{
    public void IdleState()
    {
        animator.Play("sun_Idle");
        
        doubleJump = false;
        
        if (isRunning)
        {
            state = SunWuKongState.Run;
        }
        else
        {
            if (GetInputX() != 0)
            {
                state = SunWuKongState.Walk;
            }
        }
    }
}