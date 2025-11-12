using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class SunController
{
    private float jumpTimer = 0.4f;

    private float JumpFore = 5;

    private float JumpScale = 1;

    private bool resetJumpValues = false;
    
    private void JumpState()
    {
        if (!resetJumpValues)
        {
            ResetJumpValues();
        }
        
        animator.Play("sun_jump");
        
        if (Input.GetKey(KeyCode.K))
        {
            jumpTimer -= Time.deltaTime;
            rigidbody2D.velocity = new Vector2(airSpeed * GetInputX() , JumpFore*JumpScale);
            
            JumpScale += Time.deltaTime;
            
            if (jumpTimer<0)
            {
                state = SunWuKongState.Fall;
            }
        }
        
        if (Input.GetKeyUp(KeyCode.K))
        {
            state = SunWuKongState.Fall;
        }
    }
    
    /// <summary>
    /// 重置跳跃数值
    /// </summary>
    private void ResetJumpValues()
    {
        jumpTimer = 0.4f;
        JumpFore = 5;
        JumpScale = 1;
        resetJumpValues = true;
    }
}
