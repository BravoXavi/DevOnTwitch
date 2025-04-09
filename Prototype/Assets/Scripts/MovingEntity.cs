using System;
using UnityEngine;

namespace Character.Controllers
{
    public class MovingEntity : MonoBehaviour
    {
        [SerializeField] protected Rigidbody2D _rigidbody2D;
        [SerializeField] protected BoxCollider2D _collider2D;
        [SerializeField] private SpriteRenderer _sprite;

        private bool _isLookingRight;
        private const float _flipVelocityThreshold = 0.01f;

        protected virtual void Awake()
        {
            _isLookingRight = true;
        }

        protected void FlipIfNeeded()
        {
            if (_rigidbody2D == null || _sprite == null) return;
            
            var xVelocity = _rigidbody2D.velocity.x;
            
            if (IsBetweenThreshold(xVelocity) ||
                (_isLookingRight && xVelocity >= _flipVelocityThreshold) || 
                (!_isLookingRight && xVelocity <= -_flipVelocityThreshold))
            {
                return;
            }

            _sprite.flipX = _isLookingRight;
            _isLookingRight = !_isLookingRight;
        }

        private bool IsBetweenThreshold(float value) => 
            value is < _flipVelocityThreshold and > -_flipVelocityThreshold;
    }
}