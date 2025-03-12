using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

namespace DnaCore.Character.Input
{
    public class PlayerInputSource : InputActions.IPlayerActions, IPlayerInputSource
    {
        private readonly InputActions _inputActions = new();
        
        private Buttons _pressBuffer;
        private Buttons _fixedPressBuffer;
        private PlayerInput _input;
        
        public PlayerInput FixedInput
        {
            get
            {
                _input.Press = _input.Down.ApplyPressBuffer(_fixedPressBuffer);
                _fixedPressBuffer = _input.Down;
                return _input;
            }
        }
        
        public PlayerInput Input
        {
            get
            {
                _input.Press = _input.Down.ApplyPressBuffer(_pressBuffer);
                _pressBuffer = _input.Down;
                return _input;
            }
        }

        public PlayerInputSource() => _inputActions.Player.SetCallbacks(this);
        
        public void Enable() => _inputActions.Enable();
        public void Disable() => _inputActions.Disable();
        
        ~PlayerInputSource() => _inputActions.Dispose();

        public void OnLook(CallbackContext context) => _input.LookVector = context.ReadValue<Vector2>();
        
        public void OnMove(CallbackContext context)
        {
            var vector = context.ReadValue<Vector2>();
            
            _input.Down.Up = vector.y > 0f;
            _input.Down.Down = vector.y < 0f;
            _input.Down.Right = vector.x > 0f;
            _input.Down.Left = vector.x < 0f;
        }
        
        public void OnAttack(CallbackContext context) => _input.Down.Attack = context.ReadValue<float>() > 0f;
        public void OnInteract(CallbackContext context) => _input.Down.Interact = context.ReadValue<float>() > 0f;
        public void OnJump(CallbackContext context) => _input.Down.Jump = context.ReadValue<float>() > 0f;
        public void OnSprint(CallbackContext context) => _input.Down.Sprint = context.ReadValue<float>() > 0f;
        public void OnCrouch(CallbackContext context) => _input.Down.Crouch = context.ReadValue<float>() > 0f;
        public void OnPause(CallbackContext context) => _input.Down.Pause = context.ReadValue<float>() > 0f;
    }
}
