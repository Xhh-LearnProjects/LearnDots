using Unity.Entities;
using Unity.Mathematics;
using Unity.Rendering;

namespace DOTS.Samples.Tank
{
    //Aspect 是用来操作Turret Component的
    public readonly partial struct TurretAspect : IAspect
    {

        readonly RefRO<Turret> m_Turret;

        public Entity CannonBallSpawn => m_Turret.ValueRO.CannonBallSpawn;
        public Entity CannonBallPrefab => m_Turret.ValueRO.CannonBallPrefab;
    }
}
