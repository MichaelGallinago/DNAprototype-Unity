using IK2D.Bodies;
using Unity.Entities;
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
                new BakerQuery(this, entity).AddComponents<GroundSpeed, Velocity, Gravity>();
            }
        }
    }
}
