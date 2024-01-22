using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Combat
{
    public class Health : MonoBehaviour
    {
        [SerializeField]
        private float _healthPoints = 100f;

        private Animator _animator;

        private bool _isDead = false;

        public bool IsDead => _isDead;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        public void TakeDamage(float damage)
        {
            _healthPoints = Mathf.Max(_healthPoints - damage, 0);

            if (_healthPoints == 0)
                Die();
        }

        private void Die()
        {
            if (_isDead)
                return;

            _isDead = true;
            _animator.SetTrigger("die");
        }
    }
}