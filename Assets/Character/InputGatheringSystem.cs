using Unity.Entities;
using UnityEngine;
using UnityEngine.InputSystem;


namespace Character
{
    [UpdateInGroup(typeof(InitializationSystemGroup))]
    public partial class InputGatheringSystem : SystemBase, InputActions.IPlayerActions
    {
        private EntityQuery _inputQuery;
        
        private Vector2 _movement;
        private Vector2 _looking;
        private float _firing;
        private bool _jumped;

        protected override void OnCreate()
        {
            _inputActions = new InputActions();
            _inputActions.Player.SetCallbacks(this);
            _inputQuery = GetEntityQuery(typeof(CharacterControllerInput));
        }
        
        private InputActions _inputActions;
        protected override void OnStartRunning() => _inputActions.Enable();
        protected override void OnStopRunning() => _inputActions.Disable();

        void InputActions.IPlayerActions.OnMove(InputAction.CallbackContext context) => 
            _movement = context.ReadValue<Vector2>();
        void InputActions.IPlayerActions.OnLook(InputAction.CallbackContext context) => 
            _looking = context.ReadValue<Vector2>();

        public void OnAttack(InputAction.CallbackContext context)
        {
            throw new System.NotImplementedException();
        }

        public void OnInteract(InputAction.CallbackContext context)
        {
            throw new System.NotImplementedException();
        }

        void InputActions.IPlayerActions.OnJump(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                _jumped = true;
            }
        }

        public void OnPrevious(InputAction.CallbackContext context)
        {
            throw new System.NotImplementedException();
        }

        public void OnNext(InputAction.CallbackContext context)
        {
            throw new System.NotImplementedException();
        }

        public void OnSprint(InputAction.CallbackContext context)
        {
            throw new System.NotImplementedException();
        }

        protected override void OnUpdate()
        {
            if (_inputQuery.CalculateEntityCount() == 0)
            {
                EntityManager.CreateEntity(typeof(CharacterControllerInput));
            }
            
            _inputQuery.SetSingleton(new CharacterControllerInput
            {
                Looking = _looking,
                Movement = _movement,
                Jumped = _jumped ? 1 : 0
            });
            
            _jumped = false;
        }
    }
}