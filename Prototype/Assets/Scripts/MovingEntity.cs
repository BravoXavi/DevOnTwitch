using System;
using UnityEngine;

namespace Character.Controllers
{
    public class MovingEntity : MonoBehaviour
    {
        [SerializeField] protected Rigidbody2D _rigidbody2D;
        [SerializeField] protected SpriteRenderer _sprite;

        private bool _isLookingRight;

        protected virtual void Awake()
        {
            _isLookingRight = true;
        }

        protected void FlipIfNeeded(float xMovementInput)
        {
            if (_rigidbody2D == null || _sprite == null) return;
            
            if (xMovementInput == 0 ||
                (_isLookingRight && xMovementInput > 0) || 
                (!_isLookingRight && xMovementInput < 0))
            {
                return;
            }
            
            _sprite.flipX = _isLookingRight;
            _isLookingRight = !_isLookingRight;
        }
    }
}