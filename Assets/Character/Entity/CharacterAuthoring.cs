using Character.Components;
using Character.Input;
using IK2D.Bodies;
using PhysicsEcs2D.Components;
using PhysicsEcs2D.Tiles.Collision.TileSensorEntity;
using Unity.Entities;
using UnityEngine;
using Utilities;

namespace Character
{
    public class CharacterAuthoring : MonoBehaviour
    {
        [SerializeField] private AnthroBody _body;
        [SerializeField] private PhysicsConfig _physicsConfig;
        [SerializeField] private TileSensorAuthoring _tileSensorFloor1;
        [SerializeField] private TileSensorAuthoring _tileSensorFloor2;

        private class Baker : Baker<CharacterAuthoring>
        {
            public override void Bake(CharacterAuthoring authoring) => new BakerQuery(this, TransformUsageFlags.Dynamic)
                .AddComponents<PlayerInput>() //TODO: move to player
                .AddComponents<GroundTag, GroundSpeed, Velocity, Rotation, LandEvent, AirLock>()
                .AddDisabledComponents<AirTag>()
                .AddComponents(new FloorSensors
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
                });
        }
    }
}
