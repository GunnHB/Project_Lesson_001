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

        // 공격이 시작되자마자 바로 실행되도록
        private float _timeSinceLastAttack = Mathf.Infinity;

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
                    _mover.MoveTo(_target.transform.position, 1f);
            }
        }

        private void AttackBehaviour()
        {
            transform.LookAt(_target.transform);

            if (_timeSinceLastAttack >= _timeBetweenAttacks)
            {
                TriggerAttack();
                _timeSinceLastAttack = 0f;
            }
        }

        private void TriggerAttack()
        {
            // this will trigger the Hit() event
            _animator.ResetTrigger("stopAttack");
            _animator.SetTrigger("attack");
        }

        public bool CanAttack(GameObject target)
        {
            if (target == null)
                return false;

            Health targetHealth = target.GetComponent<Health>();

            return targetHealth != null && !targetHealth.IsDead;
        }

        // Animation Event
        void Hit()
        {
            if (_target == null)
                return;

            _enemyHealth = _target.GetComponent<Health>();
            _enemyHealth.TakeDamage(_weaponDamage);
        }

        public void Attack(GameObject target)
        {
            _scheduler.StartAction(this);

            if (target.TryGetComponent(out Health targetHealth))
                _target = targetHealth;
        }

        public void Cancel()
        {
            StopAttack();
            _target = null;

            _mover.Cancel();
        }

        private void StopAttack()
        {
            _animator.ResetTrigger("attack");
            _animator.SetTrigger("stopAttack");
        }
    }
}
