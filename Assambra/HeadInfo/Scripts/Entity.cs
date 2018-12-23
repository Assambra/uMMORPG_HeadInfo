using UnityEngine;
using UnityEngine.UI;

public partial class Entity
{
    [Header("Head Info")]
    public UIHeadInfo headInfo;


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
