using DnaCore.Character.Components;
using DnaCore.Character.Input;
using DnaCore.IK2D.Bodies;
using DnaCore.PhysicsEcs2D.Components;
using DnaCore.PhysicsEcs2D.Tiles.Collision.TileSensorEntity;
using DnaCore.Utilities;
using Unity.Entities;
using UnityEngine;

namespace DnaCore.Character.Authoring
{
    public class CharacterAuthoring : MonoBehaviour
    {
        [SerializeField] private AnthroBodyAuthoring bodyAuthoring;
        [SerializeField] private Transform _physicsBody;
        [SerializeField] private PhysicsConfig _physicsConfig;
        [SerializeField] private TileSensorAuthoring _tileSensorFloor1;
        [SerializeField] private TileSensorAuthoring _tileSensorFloor2;

        private class Baker : Baker<CharacterAuthoring>
        {
            public override void Bake(CharacterAuthoring authoring) => new BakerQuery(this, TransformUsageFlags.Dynamic)
                .AddComponents<CharacterInput, GroundTag, GroundSpeed, Velocity, Rotation, LandEvent, AirLock>()
                .AddDisabledComponents<AirTag>()
                .AddComponents(
                    new FloorSensors
                    {
                        First = GetEntity(DependsOn(authoring._tileSensorFloor1), TransformUsageFlags.Dynamic),
                        Second = GetEntity(DependsOn(authoring._tileSensorFloor2), TransformUsageFlags.Dynamic)
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
                    },
                    new PhysicsBody
                    {
                        Entity = GetEntity(DependsOn(authoring._physicsBody), TransformUsageFlags.Dynamic)
                    }
                );
        }
    }
}
