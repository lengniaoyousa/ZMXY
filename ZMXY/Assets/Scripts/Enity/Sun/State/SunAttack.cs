using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class SunController
{
    private Skill skill;
    private void NewAttckSkill(int skillId)
    {
         skill = new Skill(skillId,this);
         rigidbody2D.velocity  = Vector2.zero;
    }
    
    private void SunAttackState()
    {
        Debug.Log("攻击状态");
        skill.ReleaseSkill();
        
        isOpenInputCheck = false;
        SetInputX(0);

        if (skill.IsSkillEnd)
        {
            state = SunWuKongState.Idle;
            isOpenInputCheck = true;
        }
    }
    
}
