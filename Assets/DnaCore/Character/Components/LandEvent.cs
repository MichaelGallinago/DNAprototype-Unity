using DnaCore.PhysicsEcs2D.Tiles.Collision.TileSensorEntity;
using Unity.Entities;

namespace DnaCore.Character.Components
{
    public struct LandEvent : IComponentData, IEnableableComponent
    {
        public TileSensor Sensor;
    }
}
