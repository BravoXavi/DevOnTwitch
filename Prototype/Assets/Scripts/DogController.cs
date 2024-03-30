using Global.Layers;
using UnityEngine;

public class DogController : MonoBehaviour
{
    [SerializeField] private Rigidbody2D _rigidbody2D;
    
    [SerializeField] private float _speed; 
    [SerializeField] private float _jumpForce;
    
    [SerializeField] private float _barkRadius = 3.0f;
    [SerializeField] private float _barkStrength = 200.0f;
    
    private float _xMovement = 0.0f;
    private bool _grounded = false;
    
    private void Update()
    {
        _xMovement = Input.GetAxisRaw("Horizontal");
        CheckJump();
        CheckBark();
    }

    private void FixedUpdate()
    {
        if (_rigidbody2D == null)
        {
            return;
        }

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

    private void CheckBark()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            var detectedTargets = Physics2D.OverlapCircleAll(transform.position, _barkRadius);
            foreach (var target in detectedTargets)
            {
                if (!LayerUtils.IsInLayer(target.gameObject, LayerUtils.Strings.BARKABLE))
                {
                    continue;
                }

                var barkableItem = target.gameObject.GetComponent<IBarkable>();
                barkableItem?.OnBarked(transform, _barkStrength);
            }
        }
    }

    #region DEBUG
    
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, _barkRadius);
    }
    
    #endregion
}
