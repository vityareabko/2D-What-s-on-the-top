using Services.StorageService.JsonDatas;

namespace PersistentData
{
    public interface IPersistentResourceData : IPersistentData
    {
        public ResourcesJsonData ResourcesJsonData { get; }
    }
}