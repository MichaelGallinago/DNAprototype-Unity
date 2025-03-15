using DnaCore.IK2D.Heads;
using DnaCore.IK2D.Limbs;
using UnityEngine;

namespace DnaCore.IK2D.Bodies
{
    public class AnthroBodyAuthoring : MonoBehaviour
    {
        [Header(nameof(AnthroBodyAuthoring))]
        [SerializeField] private AnthroHead _head;
        [SerializeField] private Transform _body;
        [SerializeField] private AbstractLimb _frontHand;
        [SerializeField] private AbstractLimb _backHand;
        [SerializeField] private AbstractLimb _frontLeg;
        [SerializeField] private AbstractLimb _backLeg;
    }
}
