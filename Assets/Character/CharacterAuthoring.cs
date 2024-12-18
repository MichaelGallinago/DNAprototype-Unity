using Character.Input;
using IK2D.Bodies;
using Tiles.Collision.TileSensorEntity;
using Unity.Entities;
using Unity.Mathematics;
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
            public override void Bake(CharacterAuthoring authoring)
            {
                DependsOn(authoring._tileSensorFloor1);
                DependsOn(authoring._tileSensorFloor2);
                
                new BakerQuery(this, TransformUsageFlags.Dynamic)
                    .AddComponents<GroundSpeed, Velocity, PlayerInput>()
                    .AddComponents(new Gravity { Vector = new float2(0f, -0.21875f) })
                    .AddComponents(new CharacterSensors
                    {
                        First = GetEntity(authoring._tileSensorFloor1, TransformUsageFlags.Dynamic),
                        Second = GetEntity(authoring._tileSensorFloor2, TransformUsageFlags.Dynamic)
                    });
            }
        }
    }
}
