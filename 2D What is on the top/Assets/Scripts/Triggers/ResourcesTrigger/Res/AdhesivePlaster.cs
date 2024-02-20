using System;
using UnityEngine;

namespace ResourcesCollector
{
    public class AdhesivePlaster : PickUpBase
    {
        public override ResourceStorageTypes StorageType { get; } = ResourceStorageTypes.AdhesivePlaster;
    }
}