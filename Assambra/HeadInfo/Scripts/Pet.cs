public partial class Pet
{
    protected override void UpdateHeadInfo()
    {
        base.UpdateHeadInfo();

        if (headInfo != null)
        {
            if (owner != null)
            {
                headInfo.EntityName = owner.name;
                // find local player (null while in character selection)
                if (Player.localPlayer != null)
                {
                    // note: murderer has higher priority (a player can be a murderer and an
                    // offender at the same time)
                    if (owner.IsMurderer())
                        headInfo.EntityNameColor = Player.localPlayer.nameMurdererColor;
                    else if (owner.IsOffender())
                        headInfo.EntityNameColor = Player.localPlayer.nameOffenderColor;
                    // member of the same party
                    else if (Player.localPlayer.InParty() && Player.localPlayer.party.GetMemberIndex(owner.name) != -1)
                        headInfo.EntityNameColor = Player.localPlayer.nameDefaultColor;
                    // otherwise default
                    else
                        headInfo.EntityNameColor = Player.localPlayer.nameDefaultColor;
                }
            }
            else headInfo.EntityName = "?";
        }
    }
}
