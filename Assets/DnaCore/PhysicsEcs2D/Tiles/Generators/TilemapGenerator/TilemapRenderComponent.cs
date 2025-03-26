using Unity.Entities;
using UnityEngine;

namespace DnaCore.PhysicsEcs2D.Tiles.Generators.TilemapGenerator
{
    public class TilemapRenderComponent : IComponentData
    {
        public GameObject GameObject;

        ~TilemapRenderComponent() => Object.Destroy(GameObject); //TODO: test that it fix tilemap drawing in editor
    }
}
