using UnityEngine;

namespace Character.Controllers
{
    [CreateAssetMenu]
    public class DogStatsObject : ScriptableObject
    {
        [SerializeField] private float _maxSpeed = 20f;
        [SerializeField] private float _maxSpeedDelta = 2f; 
        [SerializeField] private float _jumpForce;
        [SerializeField] private float _maxGravity = -9.8f;
        [SerializeField] private float _maxGravityDelta = 1f;
        [SerializeField] private float _collisionDistance = 2f;
        [SerializeField] private float _jumpBuffering = .35f;
        [SerializeField] private float _jumpReleasedModifier = 4.0f;
        [SerializeField] private float _coyoteTime = 0.5f;

        public float MaxSpeed => _maxSpeed;
        public float MaxSpeedDelta => _maxSpeedDelta;
        public float JumpForce => _jumpForce;
        public float MaxGravity => _maxGravity;
        public float MaxGravityDelta => _maxGravityDelta;
        public float CollisionDistance => _collisionDistance;
        public float JumpBuffering => _jumpBuffering;
        public float JumpReleasedModifier => _jumpReleasedModifier;
        public float CoyoteTime => _coyoteTime;
    }
}