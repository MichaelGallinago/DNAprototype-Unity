using System;
using Tiles;
using UnityEngine;

public class Jonkler : MonoBehaviour
{
    private void Start()
    {
        //Collider2D 
        PolygonCollider2D collider = GetComponent<PolygonCollider2D>();
        /*TileCollider tileCollider = DependencyFactory.Instance.TileColliderFactory.Create(
            Layers.Front, Quadrant.Down, Vector2Int.zero);*/
    }

    private void Update()
    {
        PhysicsMaterial material = new PhysicsMaterial();
    }
}
