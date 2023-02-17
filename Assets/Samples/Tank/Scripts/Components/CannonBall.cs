using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;

namespace DOTS.Samples.Tank
{
    public struct CannonBall : IComponentData
    {
        public float3 Speed;
    }
}
