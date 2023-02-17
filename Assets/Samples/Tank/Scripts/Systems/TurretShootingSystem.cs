using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Rendering;
using Unity.Transforms;
using Unity.Physics;

namespace DOTS.Samples.Tank
{
    [BurstCompile]
    public partial struct TurretShootingSystem : ISystem
    {
        ComponentLookup<LocalTransform> m_LocalToWorldTransformFromEntity;


        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            m_LocalToWorldTransformFromEntity = state.GetComponentLookup<LocalTransform>();
        }

        [BurstCompile]
        public void OnDestroy(ref SystemState state)
        {
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            m_LocalToWorldTransformFromEntity.Update(ref state);

            var ecbSingleton = SystemAPI.GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>();
            var ecb = ecbSingleton.CreateCommandBuffer(state.WorldUnmanaged);

            var turretShootJob = new TurretShootJob
            {
                LocalTransformFromEntity = m_LocalToWorldTransformFromEntity,
                ECB = ecb
            };

            turretShootJob.Schedule();

        }
    }


    [BurstCompile]
    partial struct TurretShootJob : IJobEntity
    {
        [ReadOnly]
        public ComponentLookup<LocalTransform> LocalTransformFromEntity;
        public EntityCommandBuffer ECB;

        void Execute(in TurretAspect turret)
        {
            var instance = ECB.Instantiate(turret.CannonBallPrefab);
            var spawnLocalToWorld = LocalTransformFromEntity[turret.CannonBallSpawn];
            var cannonBallTransform = LocalTransform.FromPosition(spawnLocalToWorld.Position);
            ECB.SetComponent(instance, new LocalTransform()
            {
                _Position = cannonBallTransform.Position,
                _Scale = cannonBallTransform._Scale,
            });

            // ECB.SetComponent(instance, new CannonBall { Speed = spawnLocalToWorld.Forward * 20.0f });
        }
    }
}
