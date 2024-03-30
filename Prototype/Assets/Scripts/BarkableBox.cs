using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarkableBox : MonoBehaviour, IBarkable
{
    [SerializeField] private Rigidbody2D _rigidbody2D;
    
    public void OnBarked(Transform barkerTransform, float barkStrength)
    {
        var emitterPosition = barkerTransform.position;
        var targetPosition = transform.position;
        
        _rigidbody2D.AddForce(new Vector2(targetPosition.x - emitterPosition.x, 
            targetPosition.y - emitterPosition.y) * barkStrength);
    }
}
