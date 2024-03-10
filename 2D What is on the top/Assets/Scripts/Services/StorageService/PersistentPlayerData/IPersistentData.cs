using Services.StorageService.JsonDatas;

namespace PersistentPlayerData
{
    public interface IPersistentData
    {
        public PlayerJsonData PlayerData { get; }
        public void SavePlayerData();
    }
}