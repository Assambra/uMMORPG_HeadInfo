﻿using UnityEngine;
using Mirror;

public partial class Player
{
    [Header("Head Info Player")]
    public string guildPrefix = "[";
    public string guildSuffix = "]";
    public Color nameDefaultColor = Color.white;
    public Color nameOffenderColor = Color.magenta;
    public Color nameMurdererColor = Color.red;
    public Color namePartyColor = new Color(0.341f, 0.965f, 0.702f);

    private bool attackMode = false;
    private bool selectMode = false;

    private Entity lastKnownTarget = null;
    private Entity lastSelectedTarget = null;

    void Start_HeadInfo()
    {
        if (isLocalPlayer)
        {
            headInfo.IsLocalPlayer = true;
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

        // find local player (null while in character selection)
        if (localPlayer != null)
        {
            // note: murderer has higher priority (a player can be a murderer and an
            // offender at the same time)
            if (IsMurderer())
                headInfo.EntityNameColor = nameMurdererColor;
            else if (IsOffender())
                headInfo.EntityNameColor = nameOffenderColor;
            // member of the same party
            else if (localPlayer.InParty() && localPlayer.party.GetMemberIndex(name) != -1)
                headInfo.EntityNameColor = namePartyColor;
            // otherwise default
            else
                headInfo.EntityNameColor = nameDefaultColor;
        }

        if (headInfo != null)
            headInfo.GuildName = guildName != "" ? guildPrefix + guildName + guildSuffix : "";
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
