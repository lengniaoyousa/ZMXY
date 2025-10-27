using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunState:State
{
    protected SkillDataConfig skillData;
    
    protected float runTimeMs;
    
    #region 跳跃

    protected int jumpCount = 0;

    #endregion
    
    protected SunController Sun;
    public SunState(SunController sun)
    {
        Sun = sun;
    }
    
    public override void Enter()
    {
        base.Enter();
    }

    public override void Execute()
    {
        base.Execute();

        if (Input.GetKey(KeyCode.D))
        {
            Sun.ChangeMianChaoXiang(1);
            Sun.SetInputX(1);
        }

        if (Input.GetKey(KeyCode.A))
        {
            Sun.ChangeMianChaoXiang(-1);
            Sun.SetInputX(-1);
        }
        

        if (Input.GetKeyUp(KeyCode.A)||Input.GetKeyUp(KeyCode.D))
        {
            Sun.SetInputX(0);
        }
        
        runTimeMs += Time.deltaTime;
    }

    public override void Exit()
    {
        base.Exit();
    }
}
