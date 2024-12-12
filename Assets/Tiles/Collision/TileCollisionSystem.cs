using Tiles.Generators;
using Unity.Entities;
using UnityEngine;

namespace Tiles.Collision
{
    public partial class TileCollisionSystem : SystemBase
    {
        protected override void OnCreate()
        {
            
        }

        protected override void OnStartRunning()
        {
            Debug.Log(SystemAPI.TryGetSingleton(out NativeTilemap tilemap)
                ? tilemap.IndexesReference.Value.Count.ToString()
                : "No data found");
        }

        protected override void OnUpdate()
        {
            //throw new System.NotImplementedException();
        }
    }
}
