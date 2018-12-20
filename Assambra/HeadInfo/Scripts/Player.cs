using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.UI;

public partial class Player
{
    private bool attackMode = false;
    private Entity lastKnownTarget;

    void UpdateClient_HeadInfo()
    {
        AttackTarget();

        if(attackMode)
        {
            target.gameObject.GetComponentInChildren<UIHeadInfo>().attackMode = true;
        }
        else
        {
            if(target != null)
                target.gameObject.GetComponentInChildren<UIHeadInfo>().attackMode = false;
            else
                if(lastKnownTarget != null)
                    lastKnownTarget.gameObject.GetComponentInChildren<UIHeadInfo>().attackMode = false;
        }
    }

    [Client]
    void AttackTarget()
    {
        if (currentSkill == -1 && useSkillWhenCloser == -1 && pendingSkill == -1)
            attackMode = false;

        if (currentSkill != -1 || useSkillWhenCloser != -1 || pendingSkill != -1)
        {
            lastKnownTarget = target;
            attackMode = true;
        }

        if (Input.GetKeyDown(KeyCode.Q) && !attackMode)   
        {
            if(target != null && target != this && target != activePet)
            {
                // attackable? => attack
                if (CanAttack(target))
                {
                    // do we have at least one skill to use here?
                    if (skills.Count > 0)
                    {
                        // then try to use that one
                        TryUseSkill(0);
                    }
                }
            }
        }
        else if (Input.GetKeyDown(KeyCode.Q) && attackMode)
        {
            CmdCancelAction();
            currentSkill = -1;
            useSkillWhenCloser = -1;
            pendingSkill = -1;
        }
    }
}
