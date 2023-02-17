using Unity.Entities;
using Unity.Rendering;

namespace DOTS.Samples.Tank
{
    public class CannonBallAuthoring : UnityEngine.MonoBehaviour
    {

    }

    public class CannonBallBaker : Baker<CannonBallAuthoring>
    {
        public override void Bake(CannonBallAuthoring authoring)
        {
            AddComponent<CannonBall>();
        }
    }
}