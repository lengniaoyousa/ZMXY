using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class SunController
{
    private void SunRun()
    {
        animator.Play("sun_run");
        
        rigidbody2D.velocity = new Vector2((runSpeed) * GetMianChaoXiang(), rigidbody2D.velocity.y);
        
        if (GetInputX() == 0)
        {
            isRunning = false;
            clickACount = 0;
            clickBCount = 0;
            state = SunWuKongState.Idle;
        }
    }
}
