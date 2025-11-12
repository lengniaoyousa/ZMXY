using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class SunController 
{
    private void FallState()
    {
        animator.Play("sun_fall");
        
        //空中移动速度
        rigidbody2D.velocity = new Vector2(airSpeed *GetInputX() , rigidbody2D.velocity.y);

        if (IsGrounded())
        {
            state = SunWuKongState.Idle;
        }
    }
}
