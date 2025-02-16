using DnaCore.Utilities;
using Unity.Mathematics;
using UnityEngine;

namespace Character
{
    [CreateAssetMenu(
        fileName = nameof(PhysicsConfig),
        menuName = ScriptableObjectUtilities.Folder + nameof(PhysicsConfig))]
    public class PhysicsConfig : ScriptableObject
    {
        [field: SerializeField] public float JumpSpeed { get; private set; }
        [field: SerializeField] public float Friction { get; private set; }
        [field: SerializeField] public float Deceleration { get; private set; }
        [field: SerializeField] public float Acceleration { get; private set; }
        [field: SerializeField] public float AccelerationAir { get; private set; }
        [field: SerializeField] public float AccelerationTop { get; private set; }
        [field: SerializeField] public float2 VelocityCap { get; private set; }
        [field: SerializeField] public float2 GravityVector { get; private set; }
    }
}
