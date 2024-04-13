using Behaviors;
using Global.Layers;
using UnityEngine;

namespace Character.Controllers
{
    public class DogController : MovingEntity, IHitable
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

            if (_rigidbody2D.velocity.x != 0)
            {
                FlipIfNeeded();
            }
            
            CheckDash();
        }
        
        //Reduce speed smoothly

        private void FixedUpdate()
        {
            if (_rigidbody2D == null)
            {
                return;
            }

            if (!_dashModule.IsDashing)
            {
                AddMovement();
            }
        }

        private void AddMovement()
        {
            _rigidbody2D.velocity = new Vector2(_xMovement * _speed, _rigidbody2D.velocity.y);
        }
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!_grounded && LayerUtils.IsInLayer(other.gameObject, LayerUtils.Strings.GROUND))
            {
                _grounded = true;
            }
        }
        
        private void OnTriggerExit2D(Collider2D other)
        {
            if (_grounded && LayerUtils.IsInLayer(other.gameObject, LayerUtils.Strings.GROUND))
            {
                _grounded = false;
            }
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
        
        public void OnHit(int damage)
        {
            if (_hpModule)
            {
                return;
            }
            
            _hpModule.ReduceHP(damage);
        }

        private void OnDeadDog()
        {
            Debug.Log("C MURIO EL PERRETE");
        }
    }  
}

