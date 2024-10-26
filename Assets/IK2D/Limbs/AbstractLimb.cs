using UnityEngine;

namespace IK2D.Limbs
{
    public abstract class AbstractLimb : MonoBehaviour
    {
        public abstract Vector2 TargetPosition { get; set; }
        public abstract float EndRotation { get; set; }
    }
}
