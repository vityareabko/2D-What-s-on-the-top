using UnityEngine;

[CreateAssetMenu(fileName = "InventoryFactory", menuName = "Factory/InventoryFactory")]
public class InventoryFactory : ScriptableObject
{
    [field: SerializeField] public InventoryResourceItem Prefab { get; private set; }

    public InventoryResourceItem Get(int amount, ResourceTypes type, Transform parent)
    {
        var instance = Instantiate(Prefab, parent);
        instance.Initialize(amount, type);

        return instance;
    }
}
