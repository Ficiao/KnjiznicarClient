using KnjiznicarDataModel.Enum;
using KnjiznicarDataModel.Message;
using Network;

namespace Overworld
{
    public class PvpMatchmakingNpc : Npc
    {
        public override void Interact()
        {
            MatchmakingRequestMessage message = new MatchmakingRequestMessage()
            {
                MatchType = MatchType.Pvp,
                NpcId = NpcId,
            };
            ClientSend.SendTCPData(message, Client.OverworldServer);
        }
    }
}
