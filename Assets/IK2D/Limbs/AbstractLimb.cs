using UnityEngine;

namespace IK2D.Limbs
{
    public abstract class AbstractLimb : MonoBehaviour
    {
        public abstract Vector2 StartPosition { get; set; }
        public abstract Vector2 EndPosition { get; set; }
        public abstract float EndRotation { get; set; }
    }
}
