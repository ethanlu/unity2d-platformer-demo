using System;
using Unity2dPlatformerCookbook.Scripts.Controls;
using Unity2dPlatformerCookbook.Scripts.Utils;
using UnityEngine;

namespace Unity2dPlatformerCookbook.Scripts.Entities.States
{
    public class AerialState : EntityState
    {
        public AerialState(Entity entity, EntityStateMachine stateMachine) : base(entity, stateMachine)
        {
        }
        
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // event delegates
        
        public void AttackAction(object sender, EventArgs args)
        {
            
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // interface methods

        public override void Enter()
        {
            base.Enter();
            
            GameInput.Instance.OnStopAction += StopAction;
            GameInput.Instance.OnMoveAction += MoveAction;
            GameInput.Instance.OnJumpAction += JumpAction;
            GameInput.Instance.OnAttackAction += AttackAction;
        }

        public override void Exit()
        {
            base.Exit();
            
            GameInput.Instance.OnStopAction -= StopAction;
            GameInput.Instance.OnMoveAction -= MoveAction;
            GameInput.Instance.OnJumpAction -= JumpAction;
            GameInput.Instance.OnAttackAction -= AttackAction;

            _airJumpCount = 0;
            _jumpVelocity = 0f;
        }

        public override void Update()
        {
            base.Update();
            
            if (_jumpVelocity > 0f)
            {
                _entity.Rigidbody2D().velocity += Vector2.up * _jumpVelocity;
                _jumpVelocity = 0f;
            }

            if (_entity.Rigidbody2D().velocity.y < 0f)
            {   // if vertical velocity is negative, then we must no longer be jumping and would be falling if we are not grounded
                _entity.EntityAnimator().Jumping(false);
                _entity.EntityAnimator().Falling(!_grounded);
            }

            if (_moveVelocity != 0f && _entity.JumpConfiguration().AerialMove)
            {   // allow some movement while in the air
                ApplyMovementWithAcceleration();
            }

            if (_moveVelocity == 0f && _entity.JumpConfiguration().AerialMove)
            {   // stopped moving but do a slight drift due to being in air
                ApplyStopWithDeceleration();
            }

            if (_grounded)
            {
                _stateMachine.ChangeState(EntityStateMachine.GroundedState);
            }
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
        }
    }
}