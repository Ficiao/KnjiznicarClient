using Assets._Scripts;
using System.Collections.Generic;

class GlobalPlayerData : SingletonPersistent<GlobalPlayerData>
{
    public string username = "";
    public int playerId = -1;
    public int level = -1;
    public List<Item> items = null;
    public int adventureLevel = -1;
    public int pvpPoints = -1;
}
