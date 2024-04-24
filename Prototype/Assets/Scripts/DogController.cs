using UnityEngine;

namespace Character.Controllers
{
    //Dash vs Jump
    //Enemy AI smooth
    //Player movement physics
    //Furniture breaking
    //HP bars
    
    public class DogController : MovingEntity
    {
        [SerializeField] private DashModule _dashModule;
        [SerializeField] private BarkModule _barkModule;
        [SerializeField] private HPModule _hpModule;
        
        [SerializeField] private float _speed; 
        [SerializeField] private float _jumpForce;
        
        private float _xMovement;
        private bool _grounded;

        protected override void Awake()
        {
            base.Awake();
            _xMovement = 0.0f;
            _grounded = true;
        }

        private void Start()
        {
            if (_hpModule != null)
            {
                _hpModule.OnDead.AddListener(OnDeadDog);
            }
        }

        private void Update()
        {
            _xMovement = Input.GetAxisRaw("Horizontal");
            CheckJump();
            CheckBark();
            CheckDash();
            
            if (_rigidbody2D.velocity.x != 0)
            {
                FlipIfNeeded();
            }
        }
        
        private void FixedUpdate()
        {
            UpdateMovement();
        }

        private void UpdateMovement()
        {
            if (!_dashModule.IsDashing)
            {
                _rigidbody2D.velocity = new Vector2((_xMovement * _speed) * Time.deltaTime, _rigidbody2D.velocity.y);
            }
        }
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (_grounded) return;
            _grounded = true;
        }
        
        private void OnTriggerExit2D(Collider2D other)
        {
            if (!_grounded) return;
            _grounded = false;
        }
        
        private void CheckJump()
        {
            if (Input.GetKeyDown(KeyCode.W) && _grounded)
            {
                _rigidbody2D.AddForce(new Vector2(0, _jumpForce));
            }
        }        
        
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
    }  
}

