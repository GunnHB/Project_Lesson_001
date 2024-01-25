using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Playables;

namespace RPG.Cinematics
{
    public class CinematicTrigger : MonoBehaviour
    {
        private PlayableDirector _director;

        private bool _alreadyTriggered = false;

        private void Start()
        {
            _director = GetComponent<PlayableDirector>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!_alreadyTriggered && other.tag == "Player")
            {
                _director.Play();
                _alreadyTriggered = true;
            }
        }
    }
}