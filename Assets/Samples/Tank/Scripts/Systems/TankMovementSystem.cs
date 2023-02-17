using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace DOTS.Samples.Tank
{
    //SystemBase 是class
    //不能用BurstComplied 可以作为Mgr
    public partial class TankMovementSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            var dt = SystemAPI.Time.DeltaTime;

            Entities
                .WithAll<Tank>()
                .ForEach((TransformAspect transform) =>
                {
                    var pos = transform.WorldPosition;

                    var angle = (0.5f + noise.cnoise(pos / 10f)) * 4.0f * math.PI;

                    var dir = float3.zero;
                    math.sincos(angle, out dir.x, out dir.z);
                    float speed = 5.0f;
                    transform.WorldPosition += dir * dt * speed;
                    transform.WorldRotation = quaternion.RotateY(angle);
                }).ScheduleParallel();
        }
    }
}
