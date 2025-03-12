using DnaCore.Character.Components;
using Unity.Entities;
using Unity.Mathematics;

namespace DnaCore.PhysicsEcs2D.Components
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
