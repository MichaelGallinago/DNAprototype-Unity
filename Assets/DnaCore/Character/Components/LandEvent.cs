using DnaCore.PhysicsEcs2D.Tiles.Collision;
using Unity.Entities;

namespace DnaCore.Character.Components
{
    public struct LandEvent : IComponentData, IEnableableComponent
    {
        public TileSensor Sensor;
    }
}
