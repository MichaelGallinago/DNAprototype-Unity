using DnaCore.Character.Components;
using DnaCore.PhysicsEcs2D.Components;
using DnaCore.PhysicsEcs2D.Systems;
using DnaCore.Utilities.Mathematics;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace DnaCore.PhysicsEcs2D.Tiles.Collision
{
    [BurstCompile]
    [UpdateAfter(typeof(TransformUpdateSystem))]
    [UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
    public partial struct SensorAdjustSystem : ISystem
    {
        private ComponentLookup<TileSensor> _tileSensorLookup;
        
        public void OnCreate(ref SystemState state)
        {
            _tileSensorLookup = state.GetComponentLookup<TileSensor>();
            state.RequireForUpdate<TileSensor>();
        }

        public void OnUpdate(ref SystemState state)
        {
            _tileSensorLookup.Update(ref state);
            state.Dependency = new SensorAdjustJob { SensorLookup = _tileSensorLookup }.Schedule(state.Dependency);
        }

        public readonly void OnDestroy(ref SystemState state) {}
    }
    
    [BurstCompile]
    public partial struct SensorAdjustJob : IJobEntity
    {
        public ComponentLookup<TileSensor> SensorLookup;
        
        private void Execute(in Rotation rotation, in LocalToWorld transform, in CharacterSensors sensors)
        {
            var position = (int2)transform.Position.xy;
            Quadrant quadrant = MathUtilities.GetQuadrant(rotation.Radians);
            var sign = (int)rotation.Facing;
            
            var centerOffset = new int2(0, 32);
            int2 floorLeftWorld = position + centerOffset + RotatePoint(new int2(-9, -32), quadrant);
            int2 floorRightWorld = position + centerOffset + RotatePoint(new int2(9, -32), quadrant);
            
            SetData(sensors.FloorLeft, floorLeftWorld, quadrant.Combine(Quadrant.Down));
            SetData(sensors.FloorRight, floorRightWorld, quadrant.Combine(Quadrant.Down));
            //SetData(sensors.WallBottom, position + new int2(sign * 13, 12), quadrant.Combine(Quadrant.Right));
            //SetData(sensors.WallTop, position + new int2(sign * 13, 52), quadrant.Combine(Quadrant.Right));
        }
        
        private static int2 RotatePoint(int2 point, Quadrant quadrant)
        {
            return quadrant switch
            {
                Quadrant.Down => point,
                Quadrant.Right => new int2(point.y, -point.x),
                Quadrant.Up => new int2(-point.x, -point.y),
                Quadrant.Left => new int2(-point.y, point.x),
                _ => point
            };
        }
        
        private void SetData(Entity entity, int2 position, Quadrant quadrant)
        {
            RefRW<TileSensor> sensor = SensorLookup.GetRefRW(entity);
            sensor.ValueRW.Position = position;
            sensor.ValueRW.Quadrant = quadrant;
        }
    }
}