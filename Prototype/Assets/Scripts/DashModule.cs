using System;
using System.Collections;
using Behaviors;
using UnityEngine;
using UnityEngine.Serialization;

namespace Character.Controllers
{
    public class DashModule : MonoBehaviour
    {
        [SerializeField] private float _dashForce;
        [SerializeField] private float _dashTime;
        [SerializeField] private float _dashCooldown;

        //[SerializeField] private GameObject _dashHitter;

        public bool IsDashing { get; private set; }
        public bool CanDash { get; private set; }

        private void Awake()
        {
            IsDashing = false;
            CanDash = true;
            //_dashHitter.SetActive(false);
        }

        public void TriggerDash(Rigidbody2D rigidbody2D)
        {
            StartCoroutine(Dash(rigidbody2D));
        }

        private IEnumerator Dash(Rigidbody2D rigidBody2D)
        {
            CanDash = false;
            IsDashing = true;

            //_dashHitter.SetActive(true);
            var originalConstraints = rigidBody2D.constraints;
            rigidBody2D.constraints |= RigidbodyConstraints2D.FreezePositionY;
            var originalGravity = rigidBody2D.gravityScale;
            rigidBody2D.gravityScale = 0;
            var movementSign = Math.Sign(transform.localScale.x);
            rigidBody2D.velocity = new Vector2((_dashForce * movementSign) * Time.deltaTime, 0);

            yield return new WaitForSeconds(_dashTime);
            
            rigidBody2D.gravityScale = originalGravity;
            rigidBody2D.constraints = originalConstraints;
            IsDashing = false;
            //_dashHitter.SetActive(false);

            yield return new WaitForSeconds(_dashCooldown);
            CanDash = true;
        }
/*
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.TryGetComponent<IHitable>(out var hitableItem))
            {
                hitableItem?.OnHit(5);
            }
        }*/
    }
}