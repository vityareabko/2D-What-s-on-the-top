using Obstacles;
using UnityEngine;

namespace Scriptable.Datas.FallResources
{
    [System.Serializable]
    public class FallingResourceConfig
    {
        [field: SerializeField] public FallingResourceType Type { get; private set; }
        [field: SerializeField] public FallObject Prefab { get; private set; }
        [field: SerializeField] public float Speed { get; set; }
    }
}