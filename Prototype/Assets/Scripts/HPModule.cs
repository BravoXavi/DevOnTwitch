using System;
using System.Collections;
using Behaviors;
using UnityEngine;
using UnityEngine.Events;

namespace Character.Controllers
{
    public class HPModule : MonoBehaviour, IHitable
    {
        [SerializeField] private int _maxHP = 100;
        [SerializeField] private int _minHP = 0;
        [SerializeField] private int _invulnerabilitySeconds;

        private int _currentHP;
        private bool _isInvulnerable = false;

        public UnityEvent OnDead;
        public UnityEvent OnDamageReceivedEvent;

        private void Awake()
        {
            _currentHP = _maxHP;
        }

        public void AddHP(int addedValue) => 
            _currentHP = Math.Min(_currentHP + addedValue, _maxHP);

        private void ReduceHP(int reducedValue)
        {
            _currentHP = Math.Max(_currentHP - reducedValue, _minHP);
            if (_currentHP == _minHP)
            {
                OnDead?.Invoke();
            }
        }

        public void OnDamageReceived(int damage = 1)
        {
            if (_isInvulnerable) return;

            Debug.Log("Damaged!!");
            ReduceHP(damage);
            OnDamageReceivedEvent?.Invoke();
            _isInvulnerable = true;
            StartCoroutine(InvulnerabilityCO(_invulnerabilitySeconds));
        }

        private IEnumerator InvulnerabilityCO(int invTime)
        {
            yield return new WaitForSeconds(invTime);
            _isInvulnerable = false;
        }
    }
}