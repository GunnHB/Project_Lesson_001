using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using RPG.Core;
using RPG.Movement;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour, IAction
    {
        [SerializeField]
        private float _weaponRange = 2f;
        [SerializeField]
        private float _timeBetweenAttacks = 1f;
        [SerializeField]
        private float _weaponDamage = 5f;

        private Health _target;
        private Mover _mover;
        private ActionScheduler _scheduler;
        private Animator _animator;
        private Health _enemyHealth;

        private float _timeSinceLastAttack = 0f;

        private void Awake()
        {
            _scheduler = GetComponent<ActionScheduler>();
            _mover = GetComponent<Mover>();
            _animator = GetComponent<Animator>();
        }

        private void Update()
        {
            _timeSinceLastAttack += Time.deltaTime;

            if (_target != null)
            {
                if (_target.IsDead)
                    return;

                var remainDistance = Vector3.Distance(transform.position, _target.transform.position);

                if (remainDistance <= _weaponRange)
                {
                    _mover.Cancel();
                    AttackBehaviour();
                }
                else
                    _mover.MoveTo(_target.transform.position);
            }
        }

        private void AttackBehaviour()
        {
            transform.LookAt(_target.transform);

            if (_timeSinceLastAttack >= _timeBetweenAttacks)
            {
                // this will trigger the Hit() event
                _animator.SetTrigger("attack");
                _timeSinceLastAttack = 0f;
            }
        }

        // Animation Event
        void Hit()
        {
            if (_target != null)
            {
                _enemyHealth = _target.GetComponent<Health>();
                _enemyHealth.TakeDamage(_weaponDamage);
            }
        }

        public void Attack(CombatTarget target)
        {
            _scheduler.StartAction(this);

            if (target.TryGetComponent(out Health targetHealth))
                _target = targetHealth;
        }

        public void Cancel()
        {
            _animator.SetTrigger("stopAttack");
            _target = null;
        }
    }
}
