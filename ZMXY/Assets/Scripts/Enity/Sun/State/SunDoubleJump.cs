using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class SunController 
{
    
    private float doublejumpTimer = 0.4f;

    private float doubleJumpFore = 4;

    private float doubleJumpScale = 1;

    private bool resetDoubleJumpValue = false;
    
    private void DoubleJump()
    {
        if (!resetDoubleJumpValue)
        {
            ResetDoubleJumpValues();
        }
        
        animator.Play("sun_doubleJump");
        
        if (Input.GetKey(KeyCode.K))
        {
            doublejumpTimer -= Time.deltaTime;
            rigidbody2D.velocity = new Vector2(airSpeed *GetInputX() , doubleJumpFore*doubleJumpScale);
            Debug.Log(GetMianChaoXiang());
            
            doubleJumpScale += Time.deltaTime;
            
            if (doublejumpTimer<0)
            {
                state = SunWuKongState.Fall;
            }
        }
        
        if (Input.GetKeyUp(KeyCode.K))
        {
            
            state = SunWuKongState.Fall;
        }
    }
    
    private void ResetDoubleJumpValues()
    {
        doublejumpTimer = 0.4f;
        doubleJumpFore = 4;
        doubleJumpScale = 1;
        resetDoubleJumpValue = true;
    }
}
