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

        private Transform _target;
        private Mover _mover;
        private ActionScheduler _scheduler;
        private Animator _animator;

        private void Awake()
        {
            _scheduler = GetComponent<ActionScheduler>();
            _mover = GetComponent<Mover>();
            _animator = GetComponent<Animator>();
        }

        private void Update()
        {
            if (_target != null)
            {
                var remainDistance = Vector3.Distance(transform.position, _target.position);

                if (remainDistance <= _weaponRange)
                {
                    _mover.Cancel();
                    AttackBehaviour();
                }
                else
                    _mover.MoveTo(_target.position);
            }
        }

        private void AttackBehaviour()
        {
            _animator.SetTrigger("attack");
        }

        public void Attack(CombatTarget target)
        {
            _scheduler.StartAction(this);
            _target = target.transform;
        }

        public void Cancel()
        {
            _target = null;
        }

        // Animation Event
        void Hit()
        {

        }
    }
}
