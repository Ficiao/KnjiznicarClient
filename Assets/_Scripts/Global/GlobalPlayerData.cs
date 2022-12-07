using KnjiznicarDataModel;

namespace Global
{
    class GlobalPlayerData : SingletonPersistent<GlobalPlayerData>
    {
        public PlayerData PlayerData = new PlayerData();
    }
}