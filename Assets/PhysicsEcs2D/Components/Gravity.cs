using Character.Components;
using Character.Input;
using PhysicsEcs2D.Systems;
using Unity.Entities;
using Unity.Mathematics;

namespace PhysicsEcs2D.Components
{
    /// <summary>
    /// Represents a gravity vector applied to an entity in the <see cref="Systems.PhysicsSystem"/>.
    /// Requires the <see cref="AirTag"/> component for processing.
    /// </summary>
    public struct Gravity : IComponentData
    {
        public float2 Vector;
    }
}
