using UnityEngine;
using Mirror;

public partial class Player
{
    private bool attackMode = false;
    private bool selectMode = false;
    private Entity lastKnownTarget = null;
    private Entity lastSelectedTarget = null;

    void Start_HeadInfo()
    {
        if (isLocalPlayer)
        {
            headInfo.IsPlayer = true;
            headInfo.AlwaysShowPlayerHealth = true;
        }
    }

    void UpdateClient_HeadInfo()
    {
        AttackTarget();

        if (target)
        {
            if(!selectMode)
            {
                lastSelectedTarget = target;
                selectMode = true;
            }
            else
            {
                target.headInfo.SelectMode = true;
            }

            if(target != lastSelectedTarget)
            {
                lastSelectedTarget.headInfo.SelectMode = false;
                selectMode = false;
            }
        }

        if (attackMode)
        {
            target.headInfo.AttackMode = true;
        }
        else
        {
            if (target != null)
                target.headInfo.AttackMode = false;
            else
                if (lastKnownTarget != null)
                lastKnownTarget.headInfo.AttackMode = false;
        }
    }

    protected override void UpdateHeadInfo()
    {
        base.UpdateHeadInfo();

        if (headInfo != null)
            headInfo.GuildName = guildName != "" ? guildOverlayPrefix + guildName + guildOverlaySuffix : "";
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
