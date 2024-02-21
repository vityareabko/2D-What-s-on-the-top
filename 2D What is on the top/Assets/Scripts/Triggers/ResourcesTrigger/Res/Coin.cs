using System;
using UnityEngine;

namespace ResourcesCollector
{
    public class Coin : PickUpBase
    {
        public override ResourceTypes Type { get; } = ResourceTypes.Coin;
    }
}