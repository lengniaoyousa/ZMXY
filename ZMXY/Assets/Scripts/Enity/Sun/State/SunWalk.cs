using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class SunController
{
    protected void WalkState()
    {
        animator.Play("sun_walk");
        if (Input.GetKey(KeyCode.A)||Input.GetKey(KeyCode.D))
        {
            rigidbody2D.velocity = new Vector2(moveSpeed * GetMianChaoXiang(), rigidbody2D.velocity.y);
        }
        else if (Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.D))
        {
            state = SunWuKongState.Idle;
        }
    }
}
