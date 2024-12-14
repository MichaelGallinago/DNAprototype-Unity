using Unity.Entities;
using UnityEngine;

namespace Character.Input
{
    [UpdateInGroup(typeof(InitializationSystemGroup))]
    public partial class InputGatheringSystem : SystemBase
    {
        private IPlayerInputSource _source;
        private EntityQuery _inputQuery;

        protected override void OnCreate()
        {
            _source = new PlayerInputSource();
            _inputQuery = SystemAPI.QueryBuilder().WithAllRW<PlayerInput>().Build();
            
            RequireForUpdate<PlayerInput>();
        }
        
        protected override void OnStartRunning() => _source.Enable();
        protected override void OnStopRunning() => _source.Disable();
        protected override void OnUpdate()
        {
            PlayerInput input = _source.PlayerInput;
            Debug.Log($"Attack: {input.Attack.Down}; Jump: {input.Jump.Down}; Interact: {input.Interact.Down}; Up: {input.Up.Down}; Down: {input.Down.Down}; Left: {input.Left.Down}; Right: {input.Right.Down}");
            Debug.Log($"Attack: {input.Attack.Press}; Jump: {input.Jump.Press}; Interact: {input.Interact.Press}; Up: {input.Up.Press}; Down: {input.Down.Press}; Left: {input.Left.Press}; Right: {input.Right.Press}");
            _inputQuery.SetSingleton(_source.PlayerInput);
        }
    }
}
