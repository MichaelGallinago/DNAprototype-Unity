using DnaCore.Utilities;
using DnaCore.Utilities.Ecs;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace DnaCore.CameraEcs
{
    public class CameraAuthoring : CameraTracker
    {
        [SerializeField] private Transform _initialTarget;
        
        private class Baker : Baker<CameraAuthoring>
        {
            public override void Bake(CameraAuthoring authoring) => new BakerQuery(this, TransformUsageFlags.Dynamic)
                .AddComponents(new CameraComponent
                {
                    Position = new float2(authoring.transform.position.x, authoring.transform.position.y),
                    Target = GetEntity(DependsOn(authoring._initialTarget), TransformUsageFlags.Dynamic)
                });
        }
    }
}
