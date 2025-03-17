using Unity.Entities;
using Unity.Transforms;

namespace DnaCore.PhysicsEcs2D.Systems
{
    [UpdateAfter(typeof(PhysicsSystem))]
    [UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
    public partial class TransformUpdateSystem : SystemBase
    {
        private TransformSystemGroup _transformSystemGroup;

        protected override void OnCreate() =>
            _transformSystemGroup = World.GetOrCreateSystemManaged<TransformSystemGroup>();

        protected override void OnUpdate()
        {
            Dependency.Complete();
            _transformSystemGroup.Update();
        }
    }
}
