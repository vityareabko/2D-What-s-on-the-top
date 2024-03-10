using System;
using UnityEngine;

namespace ResourcesCollector
{
    public class AdhesivePlaster : PickUpBase
    {
        public override ResourceTypes Type { get; } = ResourceTypes.AdhesivePlaster;
    }
}