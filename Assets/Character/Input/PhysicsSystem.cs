using Unity.Entities;

namespace Character.Input
{
    public partial struct PhysicsSystem : ISystem, ISystemStartStop
    {
        //private EntityQuery _moveableQuery;
        
        public void OnCreate(ref SystemState state)
        {
        //    _moveableQuery = SystemAPI.QueryBuilder().WithAll<PlayerInput>().Build();
        }
        
        public void OnStartRunning(ref SystemState state)
        {
        }
        
        public void OnUpdate(ref SystemState state)
        {
            
        }

        public void OnStopRunning(ref SystemState state)
        {
            
        }
        
        public void OnDestroy(ref SystemState state)
        {
        }
    }
}