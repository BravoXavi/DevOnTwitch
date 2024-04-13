using System;
using UnityEngine;
using UnityEngine.Events;

namespace Character.Controllers
{
    public class HPModule : MonoBehaviour
    {
        [SerializeField] private int _maxHP = 100;
        [SerializeField] private int _minHP = 0;

        private int _currentHP;
        public int CurrentHP => _currentHP;

        public UnityEvent OnDead;

        private void Awake()
        {
            _currentHP = _maxHP;
        }

        public void AddHP(int addedValue) => 
            _currentHP = Math.Min(_currentHP + addedValue, _maxHP);

        public void ReduceHP(int reducedValue)
        {
            _currentHP = Math.Max(_currentHP - reducedValue, _minHP);
            if (_currentHP == _minHP)
            {
                OnDead?.Invoke();
            }
        }
    }
}