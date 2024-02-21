using Obstacles;
using UnityEngine;

namespace Scriptable.Datas.FallResources
{
    [System.Serializable]
    public class FallingResourceConfig
    {
        [field: SerializeField] public ResourceTypes Type { get; set; }
        
        [field: SerializeField] public ResourceCategory CategoryType { get; private set; }
        [field: SerializeField] public FallResource Prefab { get; private set; }
        [field: SerializeField] public float Speed { get; set; }
    }
}