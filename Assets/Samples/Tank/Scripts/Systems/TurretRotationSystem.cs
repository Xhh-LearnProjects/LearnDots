using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace DOTS.Samples.Tank
{
    [BurstCompile]
    public partial struct TurretRotationSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {

        }

        [BurstCompile]
        public void OnDestroy(ref SystemState state)
        {

        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var rotation = quaternion.RotateY(SystemAPI.Time.DeltaTime * math.PI);

            foreach (var transform in SystemAPI.Query<TransformAspect>().WithAll<Turret>())
            {
                transform.RotateWorld(rotation);
            }
        }
    }
}
