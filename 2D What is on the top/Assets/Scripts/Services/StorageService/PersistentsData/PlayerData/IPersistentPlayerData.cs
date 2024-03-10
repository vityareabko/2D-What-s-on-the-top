using Services.StorageService.JsonDatas;

namespace PersistentData
{
    public interface IPersistentPlayerData : IPersistentData
    {
        public PlayerJsonData PlayerData { get; }
    }
}