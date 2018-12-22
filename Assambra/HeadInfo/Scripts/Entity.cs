using UnityEngine;
using UnityEngine.UI;

public partial class Entity
{
    [Header("Head Info")]
    public Text nameHeadInfo;
    public Text guildHeadInfo;

    void Update_HeadInfo()
    {
        if(!isServerOnly) UpdateHeadInfo();
    }

    protected virtual void UpdateHeadInfo()
    {
        if (nameHeadInfo != null)
            nameHeadInfo.text = name;
    }
}
