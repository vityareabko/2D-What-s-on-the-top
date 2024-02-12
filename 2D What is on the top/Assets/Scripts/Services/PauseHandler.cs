using System.Collections.Generic;

namespace Services
{
    public class PauseHandler : IPause
    {
        private List<IPause> _instances = new ();

        public void Add(IPause instance) => _instances.Add(instance);
        
        public void Remove(IPause instance) => _instances.Remove(instance);
        
        public void SetPause(bool isPause)
        {
            foreach (var instance in _instances)
                instance.SetPause(isPause);
        }
    }
}