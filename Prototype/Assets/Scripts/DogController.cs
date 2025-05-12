using System;
using UnityEngine;

namespace Character.Controllers
{
    public class DogController : MovingEntity
    {
        [SerializeField] private DogStatsObject _dogStats;
        [SerializeField] private DashModule _dashModule;
        [SerializeField] private BarkModule _barkModule;
        [SerializeField] private HPModule _hpModule;
        [SerializeField] private LayerMask _characterLayer;
        [SerializeField] private Animator _dogAnimator;
        
        private bool _grounded;
        private bool _jumpHeld;
        private bool _jumpPressed;
        private bool _hasToJump;
        private bool _coyoteAllowed;
        private bool _doubleJumpAllowed;
        
        private float _jumpPressedTime;
        private float _groundLeftTime;
        
        private Vector2 _movementInput;
        private Vector2 _frameVelocity;
        
        private void Start()
        {
            if (_hpModule != null)
            {
                _hpModule.OnDead.AddListener(OnDeadDog);
            }
        }

        private void Update()
        {
            GetMovementInput();
            
            CheckBark();
            CheckDash();
            
            if (_rigidbody2D.linearVelocity.x != 0)
            {
                FlipIfNeeded();
            }
        }
        
        private void FixedUpdate()
        {
            ExecuteHorizontalMovement();
            ExecuteJump();
            HandleGravity();
            HandleCollisions();
            
            _rigidbody2D.linearVelocity = _frameVelocity;
        }
        
        private void GetMovementInput()
        {
            _jumpPressed = Input.GetButtonDown("Jump") || Input.GetKeyDown(KeyCode.W);
            _jumpHeld = Input.GetButton("Jump") || Input.GetKey(KeyCode.W);
            _movementInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            
            if (_jumpPressed)
            {
                _hasToJump = true;
                _jumpPressedTime = Time.time;
            }
        }
        
        private void ExecuteHorizontalMovement()
        {
            if (_movementInput.x != 0)
            {
                var sign = Math.Sign(_movementInput.x);
                _frameVelocity.x = Mathf.MoveTowards(_frameVelocity.x, sign * _dogStats.MaxSpeed, _dogStats.MaxSpeedDelta);
                return;
            }

            if (_movementInput.x == 0)
            {
                _frameVelocity.x = Mathf.MoveTowards(_frameVelocity.x, 0, _dogStats.MaxSpeedDelta);
            }
        }
        
        private void ExecuteJump()
        {
            if (!_grounded && !_coyoteAllowed)
            {
                return;
            }
            
            if (Time.time - _jumpPressedTime > _dogStats.JumpBuffering)
            {
                _hasToJump = false;
                return;
            }
                
            if (_hasToJump)
            {
                _dogAnimator.SetBool("Jumping", true);
                _frameVelocity.y = _dogStats.JumpForce;
                _hasToJump = false;
                _coyoteAllowed = false;
            }
        }

        private void HandleGravity()
        {
            var shouldApplyModifier = !_jumpHeld && !_grounded && _frameVelocity.y > 0.0f;
            _frameVelocity.y = Mathf.MoveTowards(
                _frameVelocity.y,
                _dogStats.MaxGravity, 
                shouldApplyModifier ? _dogStats.MaxGravityDelta * _dogStats.JumpReleasedModifier 
                    : _dogStats.MaxGravityDelta );

            if (_coyoteAllowed && !_grounded && Time.time - _groundLeftTime > _dogStats.CoyoteTime)
            {
                _coyoteAllowed = false;
            }
        }
        
        private void HandleCollisions()
        {
            bool groundHit = Physics2D.BoxCast(
                _collider2D.bounds.center, 
                _collider2D.size/2.0f, 0, Vector2.down, _dogStats.CollisionDistance, _characterLayer);
            
            bool ceilingHit = Physics2D.BoxCast(
                _collider2D.bounds.center, 
                _collider2D.size/2.0f, 0, Vector2.up, _dogStats.CollisionDistance, _characterLayer);

            if (ceilingHit)
            {
                _frameVelocity.y = Mathf.Min(0, _frameVelocity.y);
            }

            if (!_grounded && groundHit)
            {
                _grounded = true;
                _coyoteAllowed = true;
                _dogAnimator.SetBool("Jumping", false);
            }

            if (_grounded && !groundHit)
            {
                _grounded = false;
                _groundLeftTime = Time.time;
            }
        }

        #region Dog Moveset
        
        private void CheckDash()
        {
            if (Input.GetKeyDown(KeyCode.R) && _dashModule.CanDash)
            {
                _dashModule.TriggerDash(_rigidbody2D);
            }
        }

        private void CheckBark()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                _barkModule.TriggerBark();
            }
        }
        
        private void OnDeadDog()
        {
            Debug.Log("C MURIO EL PERRETE");
        }
        
        #endregion
    }
}

