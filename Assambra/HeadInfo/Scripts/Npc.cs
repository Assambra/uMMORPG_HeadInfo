using System.Linq;

public partial class Npc
{
    protected override void UpdateHeadInfo()
    {
        base.UpdateHeadInfo();

        if (headInfo != null)
        {
            // find local player (null while in character selection)
            if (Player.localPlayer != null)
            {
                if (quests.Any(q => Player.localPlayer.CanCompleteQuest(q.name)))
                    headInfo.QuestSign = "!";
                else if (quests.Any(Player.localPlayer.CanAcceptQuest))
                    headInfo.QuestSign = "?";
                else
                    headInfo.QuestSign = "";
            }
        }
    }
}
