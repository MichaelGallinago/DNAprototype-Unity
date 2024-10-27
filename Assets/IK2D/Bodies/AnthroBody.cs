using IK2D.Limbs;
using UnityEngine;

namespace IK2D.Bodies
{
    public class AnthroBody : MonoBehaviour
    {
        [Header(nameof(AnthroBody))]
        [SerializeField] private Transform _head;
        [SerializeField] private Transform _body;
        [SerializeField] private AbstractLimb _frontHand;
        [SerializeField] private AbstractLimb _backHand;
        [SerializeField] private AbstractLimb _frontLeg;
        [SerializeField] private AbstractLimb _backLeg;
        
        void Start()
        {
        }
        
        void Update()
        {
            
        }
    }
}
