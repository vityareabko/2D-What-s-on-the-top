using System;

namespace ResourcesCollector
{
    public interface IPickUp
    {
        public ResourceTypes Type { get; }

        public int AmountResources { get; }
        
        // public event Action<IPickUp> PickUP;
        
    }
}