using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZM.ZMAsset;

public class SunAttackState:SunGroundState
{
    private static readonly int Attack = Animator.StringToHash("Attack");

    public SunAttackState(SunController sun) : base(sun)
    {
        
    }

    public override void Enter()
    {
        base.Enter();
        skillData = ZMAsset.LoadScriptableObject<SkillDataConfig>(AssetPath.SKILL_DATA_PATH + "/1000"+ ".asset");
        Sun.animator.SetBool(Attack,true);
        runTimeMs = 0;
    }

    public override void Execute()
    {
        base.Execute();
        
        if (runTimeMs >= skillData.character.AnimLength)
        {
            Sun.stateMachine.ChangeState<SunIdleState>();
        }
    }

    public override void Exit()
    {
        base.Exit();
        Sun.animator.SetBool(Attack,false);
    }
}
