using Character.Input;
using IK2D.Bodies;
using Tiles.Collision.TileSensorEntity;
using Unity.Entities;
using UnityEngine;
using Utilities;

namespace Character
{
    public class CharacterAuthoring : MonoBehaviour
    {
        [SerializeField] private AnthroBody _body;
        [SerializeField] private TileSensorAuthoring _tileSensorFloor1;
        [SerializeField] private TileSensorAuthoring _tileSensorFloor2;

        private class Baker : Baker<CharacterAuthoring>
        {
            public override void Bake(CharacterAuthoring authoring) => new BakerQuery(this, TransformUsageFlags.Dynamic)
                .AddComponents<PlayerInput>() //TODO: move to player
                .AddComponents<GroundSpeed, Velocity, Rotation>()
                .AddComponents(new Gravity { Vector = PhysicsConstants.GravityVector })
                .AddComponents(new Character.FloorSensors
                {
                    First = GetEntity(DependsOn(authoring._tileSensorFloor1), TransformUsageFlags.Dynamic),
                    Second = GetEntity(DependsOn(authoring._tileSensorFloor2), TransformUsageFlags.Dynamic)
                }, 
                new Character.BehaviourTree { Behaviour = Character.Behaviours.Ground });
        }
    }
}
