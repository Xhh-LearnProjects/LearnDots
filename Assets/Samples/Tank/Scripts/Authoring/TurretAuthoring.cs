using UnityEngine;
using Unity.Entities;

namespace DOTS.Samples.Tank
{
    public class TurretAuthoring : MonoBehaviour
    {
        public GameObject CannonBallPrefab;
        public GameObject CannonBallSpawn;
    }


    class TurretBaker : Baker<TurretAuthoring>
    {
        public override void Bake(TurretAuthoring authoring)
        {
            AddComponent(new Turret
            {
                CannonBallPrefab = GetEntity(authoring.CannonBallPrefab),
                CannonBallSpawn = GetEntity(authoring.CannonBallSpawn)
            });
        }
    }
}
