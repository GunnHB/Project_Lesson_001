using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using RPG.Combat;
using RPG.Movement;
using RPG.Core;
using System;

namespace RPG.Control
{
    public class AIController : MonoBehaviour
    {
        [SerializeField]
        private float _chaseDistance = 5f;
        [SerializeField]
        private float _suspicionTime = 3f;
        [SerializeField]
        private PatrolPath _patrolPath;
        [SerializeField]
        private float _waypointToTolerance = 1f;
        [SerializeField]
        private float _waypointDwellTime = 3f;

        private GameObject _playerGameObject;

        private Mover _mover;
        private Fighter _fighter;
        private Health _health;
        private ActionScheduler _scheduler;

        private Vector3 _guardPosition;

        private float _timeSinceLastSawPlayer = Mathf.Infinity;
        private float _timeSinceArrivedAtWaypoint = Mathf.Infinity;

        private int _currentWaypointIndex = 0;

        private void Awake()
        {
            _fighter = GetComponent<Fighter>();
            _health = GetComponent<Health>();
            _scheduler = GetComponent<ActionScheduler>();
            _mover = GetComponent<Mover>();

            _playerGameObject = GameObject.FindWithTag("Player");

            _guardPosition = transform.position;
        }

        private void Update()
        {
            if (_health.IsDead)
                return;

            if (InAttackRangeOfPlayer() && _fighter.CanAttack(_playerGameObject))
            {
                AttackBehaviour();
            }
            else if (_timeSinceLastSawPlayer <= _suspicionTime)
            {
                SuspicionBehaviour();
            }
            else
            {
                PatrolBahaviour();
            }

            UpdateTimers();
        }

        private void UpdateTimers()
        {
            _timeSinceLastSawPlayer += Time.deltaTime;
            _timeSinceArrivedAtWaypoint += Time.deltaTime;
        }

        private void PatrolBahaviour()
        {
            Vector3 nextPosition = _guardPosition;

            if (_patrolPath != null)
            {
                if (AtWaytPoint())
                {
                    CycleWayPoint();
                }

                nextPosition = GetCurrentWayPoint();
            }

            if (_timeSinceArrivedAtWaypoint > _waypointDwellTime)
                _mover.StartMoveAction(nextPosition);
        }

        private bool AtWaytPoint()
        {
            float distanceToWaypoint = Vector3.Distance(transform.position, GetCurrentWayPoint());
            return distanceToWaypoint < _waypointToTolerance;
        }

        private void CycleWayPoint()
        {
            _timeSinceArrivedAtWaypoint = 0;
            _currentWaypointIndex = _patrolPath.GetNextIndex(_currentWaypointIndex);
        }

        private Vector3 GetCurrentWayPoint()
        {
            return _patrolPath.GetWayPoint(_currentWaypointIndex);
        }

        private void SuspicionBehaviour()
        {
            _scheduler.CancelCurrentAction();
        }

        private void AttackBehaviour()
        {
            _timeSinceLastSawPlayer = 0;
            _fighter.Attack(_playerGameObject);
        }

        private bool InAttackRangeOfPlayer()
        {
            float distanceToPlayer = Vector3.Distance(transform.position, _playerGameObject.transform.position);
            return distanceToPlayer <= _chaseDistance;
        }

        // called by unity
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, _chaseDistance);
        }
    }
}
