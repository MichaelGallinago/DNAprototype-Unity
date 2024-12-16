using Character.Input;
using IK2D.Bodies;
using Tiles.Models;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
using Utilities;

namespace Character
{
    public class CharacterAuthoring : MonoBehaviour
    {
        [SerializeField] private AnthroBody _body;

        private class Baker : Baker<CharacterAuthoring>
        {
            public override void Bake(CharacterAuthoring authoring)
            {
                Entity entity = GetEntity(TransformUsageFlags.Dynamic);
                new BakerQuery(this, entity)
                    .AddComponents<GroundSpeed, Velocity, PlayerInput>()
                    .AddComponents(
                        new Gravity { Vector = new float2(0f, -0.21875f) },
                        new TileSensor { Quadrant = Quadrant.Down, Offset = new int2(-9, -9) },
                        new TileSensor { Quadrant = Quadrant.Down, Offset = new int2(9, -9) }
                    );
            }
        }
    }
}
