using System;
using UnityEngine;

namespace ResourcesCollector
{
    public class Coin : PickUpBase
    {
        public override ResourceStorageTypes StorageType { get; } = ResourceStorageTypes.Coin;
    }
}