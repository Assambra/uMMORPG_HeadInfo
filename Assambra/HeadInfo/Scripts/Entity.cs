using UnityEngine;
using UnityEngine.UI;

public partial class Entity
{
    [Header("Head Info")]
    public UIHeadInfo headInfo;

    private bool isNpc = false;
    private Npc npc;

    void Awake_HeadInfo()
    {
        npc = gameObject.GetComponent<Npc>();
        if (npc != null)
            headInfo.IsNpc = true;
    }

    void Update_HeadInfo()
    {
        if(!isServerOnly) UpdateHeadInfo();
    }

    protected virtual void UpdateHeadInfo()
    {
        if (headInfo != null)
            headInfo.EntityName = name;
    }
}
