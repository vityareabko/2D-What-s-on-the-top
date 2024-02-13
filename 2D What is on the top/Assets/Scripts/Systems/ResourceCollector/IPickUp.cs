using System;

namespace ResourcesCollector
{
    public interface IPickUp
    {
        public ResourceTypes Type { get; }

        public event Action<IPickUp> PickUP;
        
        public int GetCoinsValue();
    }
}