using Unity.Entities;

namespace DOTS.Samples.Tank
{
    //一个空的component 被称为tag component
    public struct Turret : IComponentData
    {
        //CannonBall Spawn Point
        public Entity CannonBallSpawn;

        public Entity CannonBallPrefab;
    }
}
