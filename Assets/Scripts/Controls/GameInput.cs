using Common.Events;
using System;
using UnityEngine;

namespace Controls
{
    public sealed class GameInput
    {
        public event EventHandler OnMoveAction;
        public event EventHandler OnStopAction;
        public event EventHandler OnJumpAction;
        public event EventHandler OnAttackAction; 
        
        private static GameInput _instance = null;
        private InputActions _inputActions;

        private bool _normalizedMovements;

        private GameInput()
        {
            _inputActions = new InputActions();
            _inputActions.Player.Enable();

            _normalizedMovements = true;

            _inputActions.Player.Move.performed += MoveEvent;
            _inputActions.Player.Move.canceled += StopEvent;
            _inputActions.Player.Jump.performed += JumpEvent;
            _inputActions.Player.Attack.performed += AttackEvent;
        }

        private void MoveEvent(UnityEngine.InputSystem.InputAction.CallbackContext args)
        {
            if (OnMoveAction is not null)
            {
                ControlMoveEventArgs a = new ControlMoveEventArgs();
                a.Value = _normalizedMovements ? args.ReadValue<Vector2>().normalized : args.ReadValue<Vector2>();
                OnMoveAction(this, a);
            }
        }
        
        private void StopEvent(UnityEngine.InputSystem.InputAction.CallbackContext args)
        {
            OnStopAction?.Invoke(this, EventArgs.Empty);
        }

        private void JumpEvent(UnityEngine.InputSystem.InputAction.CallbackContext args)
        {
            OnJumpAction?.Invoke(this, EventArgs.Empty);
        }

        private void AttackEvent(UnityEngine.InputSystem.InputAction.CallbackContext args)
        {
            OnAttackAction?.Invoke(this, EventArgs.Empty);
        }

        public static GameInput Instance {
            get
            {
                if (_instance == null)
                {
                    _instance = new GameInput();
                }
                return _instance;
            }
        }

        public void NormalizedMovement(bool toggle) { _normalizedMovements = toggle; }
    }
}