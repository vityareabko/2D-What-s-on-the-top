using System;

namespace ResourcesCollector
{
    public interface IPickUp
    {
        public ResourceStorageTypes StorageType { get; }

        public int AmountResources { get; }
        
        // public event Action<IPickUp> PickUP;
        
    }
}