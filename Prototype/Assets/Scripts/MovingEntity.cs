using System;
using UnityEngine;

namespace Character.Controllers
{
    public class MovingEntity : MonoBehaviour
    {
        [SerializeField] protected Rigidbody2D _rigidbody2D;
        [SerializeField] private SpriteRenderer _sprite;

        private bool _isLookingRight;

        protected virtual void Awake()
        {
            _isLookingRight = true;
        }

        protected void FlipIfNeeded()
        {
            if (_rigidbody2D == null || _sprite == null) return;
            
            var xVelocity = _rigidbody2D.velocity.x;
            if (xVelocity == 0.0f || 
                _isLookingRight && xVelocity >= 0.01f || 
                !_isLookingRight && xVelocity <= -0.01f)
            {
                return;
            }

            _sprite.flipX = _isLookingRight;
            _isLookingRight = !_isLookingRight;
        }
    }
}