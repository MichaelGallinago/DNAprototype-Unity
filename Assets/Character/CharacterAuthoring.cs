using Character.Input;
using IK2D.Bodies;
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
                new BakerQuery(this, TransformUsageFlags.Dynamic)
                    .AddComponents<GroundSpeed, Velocity, PlayerInput>()
                    .AddComponents(new Gravity { Vector = new float2(0f, -0.21875f) });
            }
        }
    }
}
