using UnityEngine;

namespace DnaCore.IK2D.Bodies
{
    public class DragonBodyAuthoring : AnthroBodyAuthoring
    {
        [Header(nameof(DragonBodyAuthoring))]
        [SerializeField] private Transform _tail;
        [SerializeField] private Transform _wings;
    }
}
