using System;

namespace ResourceCollector
{
    public interface IPickUp
    {
        public ResourceTypes Type { get; }

        public event Action<IPickUp> PickUP;
        
        public int GetValue();
    }
}