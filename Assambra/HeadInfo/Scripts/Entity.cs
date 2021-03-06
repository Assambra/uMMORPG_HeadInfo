﻿using UnityEngine;
using UnityEngine.UI;

public partial class Entity
{
    [Header("Head Info")]
    public bool AlwaysShowHealth = true;
    public UIHeadInfo headInfo;
    
    private Npc npc;
    private Mount mount;

    void Awake_HeadInfo()
    {
        npc = gameObject.GetComponent<Npc>();
        if (npc != null)
            headInfo.IsNpc = true;
        mount = gameObject.GetComponent<Mount>();
        if (mount != null)
            headInfo.IsMount = true;
    }

    void Update_HeadInfo()
    {
        if(!isServerOnly) UpdateHeadInfo();
    }

    protected virtual void UpdateHeadInfo()
    {
        headInfo.AlwaysShowHealth = AlwaysShowHealth;

        if (headInfo != null)
            headInfo.EntityName = name;
        if(headInfo != null)
            headInfo.IsStunned = state == "STUNNED";
    }
}
