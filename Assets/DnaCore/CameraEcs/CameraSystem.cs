using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace CameraEcs
{
    [BurstCompile]
    [UpdateInGroup(typeof(TransformSystemGroup), OrderFirst = true)] // TODO: make separate group?
    public partial struct CameraSystem : ISystem
    {
        private ComponentLookup<LocalTransform> _transformLookup;

        public void OnCreate(ref SystemState state)
        {
            _transformLookup = state.GetComponentLookup<LocalTransform>();
            state.RequireForUpdate<CameraComponent>();
        }

        public void OnUpdate(ref SystemState state)
        {
            _transformLookup.Update(ref state);
            state.Dependency = new CameraJob { TransformLookup = _transformLookup }.Schedule(state.Dependency);
        }
        
        public void OnDestroy(ref SystemState state) {}
    }
    
    [BurstCompile]
    public partial struct CameraJob : IJobEntity
    {
        public ComponentLookup<LocalTransform> TransformLookup;
        
        private void Execute(ref CameraComponent camera, in Entity entity)
        {
            float2 newPosition = TransformLookup[camera.Target].Position.xy;
            camera.Position = newPosition;
            
            LocalTransform cameraTransform = TransformLookup[entity];
            cameraTransform.Position = new float3(newPosition.x, newPosition.y, cameraTransform.Position.z);
            TransformLookup[entity] = cameraTransform;
        }
    }
}
