using System;
using UnityEngine;

namespace Character.Controllers
{
    public class MovingEntity : MonoBehaviour
    {
        [SerializeField] protected Rigidbody2D _rigidbody2D;

        private bool _isLookingRight;

        protected virtual void Awake()
        {
            _isLookingRight = true;
        }

        protected void FlipIfNeeded()
        {
            int velocitySign = Math.Sign(_rigidbody2D.velocity.x);
            
            if (_isLookingRight && velocitySign > 0 || 
                !_isLookingRight && velocitySign < 0)
            {
                return;
            }
            
            var currentScale = transform.localScale;
            transform.localScale = new Vector2(-1 * currentScale.x, currentScale.y);
            _isLookingRight = !_isLookingRight;
        }
    }
}