using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using UnityEngine.AI;

using RPG.Core;

namespace RPG.Movement
{
    public class Mover : MonoBehaviour, IAction
    {
        [SerializeField]
        private NavMeshAgent _agent;
        [SerializeField]
        private Animator _animator;
        [SerializeField]
        private float _maxSpeed;

        private ActionScheduler _scheduler;
        private Health _health;

        private void Awake()
        {
            _scheduler = GetComponent<ActionScheduler>();
            _health = GetComponent<Health>();
        }

        private void Update()
        {
            _agent.enabled = !_health.IsDead;

            UpdateAnimator();
        }

        public void StartMoveAction(Vector3 destination, float speedFraction)
        {
            _scheduler.StartAction(this);
            MoveTo(destination, speedFraction);
        }

        public void MoveTo(Vector3 destination, float speedFraction)
        {
            _agent.SetDestination(destination);
            _agent.speed = _maxSpeed * Mathf.Clamp01(speedFraction);
            _agent.isStopped = false;
        }

        public void Cancel()
        {
            _agent.isStopped = true;
        }

        private void UpdateAnimator()
        {
            Vector3 velocity = _agent.velocity;
            Vector3 localVelocity = transform.InverseTransformDirection(velocity);
            float speed = localVelocity.z;

            _animator.SetFloat("ForwardSpeed", speed);
        }
    }

}