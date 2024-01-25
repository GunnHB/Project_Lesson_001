using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Playables;

using RPG.Core;
using RPG.Control;

namespace RPG.Cinematics
{
    public class CinematicControlRemover : MonoBehaviour
    {
        private PlayableDirector _director;
        private ActionScheduler _playerScheduler;
        private PlayerController _player;

        private void Start()
        {
            GameObject playerObj = GameObject.FindWithTag("Player");

            _director = GetComponent<PlayableDirector>();

            _playerScheduler = playerObj.GetComponent<ActionScheduler>();
            _player = playerObj.GetComponent<PlayerController>();

            _director.played += DisableControl;
            _director.stopped += EnableControl;
        }

        private void DisableControl(PlayableDirector director)
        {
            print("DisableControl");

            _playerScheduler.CancelCurrentAction();
            _player.enabled = false;
        }

        private void EnableControl(PlayableDirector director)
        {
            print("EnableControl");

            _player.enabled = true;
        }
    }
}