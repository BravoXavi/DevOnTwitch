using System;
using System.Collections;
using UnityEngine;

namespace Character.Controllers
{
    public class DashModule : MonoBehaviour
    {
        [SerializeField] private float _dashForce;
        [SerializeField] private float _dashTime;
        [SerializeField] private float _dashCooldown;
        
        private bool _isDashing;
        private bool _canDash;

        public bool IsDashing => _isDashing;
        public bool CanDash => _canDash;

        private void Awake()
        {
            _isDashing = false;
            _canDash = true;
        }

        public void TriggerDash(Rigidbody2D rigidbody2D)
        {
            StartCoroutine(Dash(rigidbody2D));
        }

        private IEnumerator Dash(Rigidbody2D rigidBody2D)
        {
            _canDash = false;
            _isDashing = true;
            
            var originalGravity = rigidBody2D.gravityScale;
            rigidBody2D.gravityScale = 0;
            int movementSign = Math.Sign(transform.localScale.x);
            rigidBody2D.velocity = new Vector2(_dashForce * movementSign, 0);

            yield return new WaitForSeconds(_dashTime);
            
            rigidBody2D.gravityScale = originalGravity;
            _isDashing = false;

            yield return new WaitForSeconds(_dashCooldown);
            _canDash = true;
        }
    }
}