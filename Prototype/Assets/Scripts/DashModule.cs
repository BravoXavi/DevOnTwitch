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

        public bool IsDashing { get; private set; }
        public bool CanDash { get; private set; }

        private void Awake()
        {
            IsDashing = false;
            CanDash = true;
        }

        public void TriggerDash(Rigidbody2D rigidbody2D)
        {
            StartCoroutine(Dash(rigidbody2D));
        }

        private IEnumerator Dash(Rigidbody2D rigidBody2D)
        {
            CanDash = false;
            IsDashing = true;
            
            var originalGravity = rigidBody2D.gravityScale;
            rigidBody2D.gravityScale = 0;
            var movementSign = Math.Sign(transform.localScale.x);
            rigidBody2D.velocity = new Vector2((_dashForce * movementSign) * Time.deltaTime, 0);

            yield return new WaitForSeconds(_dashTime);
            
            rigidBody2D.gravityScale = originalGravity;
            IsDashing = false;

            yield return new WaitForSeconds(_dashCooldown);
            CanDash = true;
        }
    }
}