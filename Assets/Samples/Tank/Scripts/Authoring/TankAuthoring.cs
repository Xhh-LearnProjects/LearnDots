using UnityEngine;
using Unity.Entities;

namespace DOTS.Samples.Tank
{
    public class TankAuthoring : MonoBehaviour
    {

    }

    class TankBaker : Baker<TankAuthoring>
    {
        public override void Bake(TankAuthoring authoring)
        {
            AddComponent<Tank>();
        }
    }
}
