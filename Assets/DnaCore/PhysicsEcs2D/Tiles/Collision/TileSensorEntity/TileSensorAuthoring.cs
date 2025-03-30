using DnaCore.Utilities.Ecs;
using DnaCore.Utilities.Mathematics;
using Unity.Entities;
using UnityEngine;

namespace DnaCore.PhysicsEcs2D.Tiles.Collision.TileSensorEntity
{
    public class TileSensorAuthoring : MonoBehaviour
    {
        [SerializeField] private Quadrant _quadrant;
        
        private class Baker : Baker<TileSensorAuthoring>
        {
            public override void Bake(TileSensorAuthoring authoring) => this.BakeQuery(TransformUsageFlags.Dynamic)
                .AddComponents(new TileSensor { Quadrant = authoring._quadrant });
        }
    }
}
