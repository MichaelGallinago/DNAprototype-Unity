using PhysicsEcs2D.Tiles.Collision.TileSensorEntity;
using Unity.Entities;

namespace Character.Components
{
    public struct LandEvent : IComponentData
    {
        public TileSensor Sensor;
    }
}
