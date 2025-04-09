using DnaCore.Character.Components;
using DnaCore.Character.Input;
using DnaCore.IK2D.Bodies;
using DnaCore.PhysicsEcs2D.Components;
using DnaCore.PhysicsEcs2D.Tiles.Collision;
using DnaCore.Utilities.Ecs;
using Unity.Entities;
using UnityEngine;

namespace DnaCore.Character.Authoring
{
    public class CharacterAuthoring : MonoBehaviour
    {
        [SerializeField] private AnthroBodyAuthoring _bodyAuthoring;
        [SerializeField] private PhysicsConfig _physicsConfig;

        private class Baker : Baker<CharacterAuthoring>
        {
            public override void Bake(CharacterAuthoring authoring) => this.BakeQuery(TransformUsageFlags.Dynamic)
                .AddComponents<CharacterInput, GroundTag, GroundSpeed, Velocity, Rotation, LandEvent, AirLock>()
                .AddDisabledComponents<AirTag>()
                .AddComponents(
                    new CharacterSensors
                    {
                        FloorLeft = this.BakeQueryAdditional(TransformUsageFlags.None).AddComponents<TileSensor>(),
                        FloorRight = this.BakeQueryAdditional(TransformUsageFlags.None).AddComponents<TileSensor>(),
                        //WallBottom = this.BakeQueryAdditional(TransformUsageFlags.None).AddComponents<TileSensor>(),
                        //WallTop = this.BakeQueryAdditional(TransformUsageFlags.None).AddComponents<TileSensor>()
                    },
                    new BehaviourTree { Current = Behaviours.Ground },
                    new Gravity { Vector = authoring._physicsConfig.GravityVector },
                    new Jump { Speed = authoring._physicsConfig.JumpSpeed },
                    new PhysicsData
                    {
                        Friction = authoring._physicsConfig.Friction,
                        Deceleration = authoring._physicsConfig.Deceleration,
                        Acceleration = authoring._physicsConfig.Acceleration,
                        AccelerationAir = authoring._physicsConfig.AccelerationAir,
                        AccelerationTop = authoring._physicsConfig.AccelerationTop,
                        VelocityCap = authoring._physicsConfig.VelocityCap
                    }
                );
        }
    }
}
