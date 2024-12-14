using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

namespace Character.Input
{
    public class PlayerInputSource : InputActions.IPlayerActions, IPlayerInputSource
    {
        private readonly InputActions _inputActions = new();
        
        private PlayerInput _playerInput;
        public PlayerInput PlayerInput => _playerInput;

        public PlayerInputSource() => _inputActions.Player.SetCallbacks(this);
        
        public void Enable() => _inputActions.Enable();
        public void Disable() => _inputActions.Disable();
        
        ~PlayerInputSource() => _inputActions.Dispose();

        public void OnMove(CallbackContext context)
        {
            var vector = context.ReadValue<Vector2>();
            
            _playerInput.Up.Set(vector.y > 0f);
            _playerInput.Down.Set(vector.y < 0f);
            _playerInput.Right.Set(vector.x > 0f);
            _playerInput.Left.Set(vector.x < 0f);
        }

        public void OnLook(CallbackContext context) => _playerInput.LookVector = context.ReadValue<Vector2>();
        public void OnAttack(CallbackContext context) => _playerInput.Attack.Set(context.ReadValue<bool>());
        public void OnInteract(CallbackContext context) => _playerInput.Interact.Set(context.ReadValue<bool>());
        public void OnJump(CallbackContext context) => _playerInput.Jump.Set(context.ReadValue<bool>());
        public void OnSprint(CallbackContext context) => _playerInput.Sprint.Set(context.ReadValue<bool>());
        public void OnCrouch(CallbackContext context) => _playerInput.Crouch.Set(context.ReadValue<bool>());
        public void OnPause(CallbackContext context) => _playerInput.Pause.Set(context.ReadValue<bool>());
    }
}
