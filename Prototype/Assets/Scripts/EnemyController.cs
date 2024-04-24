using System;
using Behaviors;
using Character.Controllers;
using UnityEngine;

public class EnemyController : MovingEntity
{
    [SerializeField] private GameObject _mainCharacter;
    [SerializeField] private int _enemySpeed;
    
    private void Update()
    {
        if (_rigidbody2D.velocity.x != 0)
        {
            FlipIfNeeded();
        }
    }

    private void FixedUpdate()
    {
        TryChasePlayer();
    }

    private void TryChasePlayer()
    {
        var mainCharacterPosition = _mainCharacter.transform.position;
        var currentPosition = transform.position;
        var directionVector = new Vector2(mainCharacterPosition.x - currentPosition.x,
                mainCharacterPosition.y - currentPosition.y).normalized;

        _rigidbody2D.velocity = directionVector * _enemySpeed;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent<IHitable>(out var hittedItem))
        {
            hittedItem.OnDamageReceived();
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.TryGetComponent<IHitable>(out var hittedItem))
        {
            hittedItem.OnDamageReceived();
        }
    }
}
