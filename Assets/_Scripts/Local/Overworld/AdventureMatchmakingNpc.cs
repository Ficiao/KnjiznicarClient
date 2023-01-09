namespace Overworld
{
    public class AdventureMatchmakingNpc : Npc
    {
        public override void Interact()
        {
            UIManager.Instance.ShowAdventureMenu(NpcId);
        }
    }
}
