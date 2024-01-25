using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

namespace RPG.SceneManagement
{
    enum DestinationIdendifier
    {
        A,
        B,
    }

    public class Portal : MonoBehaviour
    {
        [SerializeField]
        private int _sceneToLoad = -1;
        [SerializeField]
        private Transform _spawnPoint;
        [SerializeField]
        private DestinationIdendifier _destination;
        [SerializeField]
        private float _fadeOutTime = 2f;
        [SerializeField]
        private float _fadeInTime = 2f;
        [SerializeField]
        private float _fadeWaitTime = .5f;

        private Coroutine _sceneTransitionCoroutine;

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Player")
            {
                if (_sceneTransitionCoroutine != null)
                {
                    StopCoroutine(_sceneTransitionCoroutine);
                    _sceneTransitionCoroutine = null;
                }

                _sceneTransitionCoroutine = StartCoroutine(nameof(Cor_Transition));
            }
        }

        private IEnumerator Cor_Transition()
        {
            if (_sceneToLoad < 0)
            {
                Debug.LogError("Scene to load not set");
                yield break;
            }

            DontDestroyOnLoad(this.gameObject);

            Fader fader = FindObjectOfType<Fader>();

            yield return fader.Cor_FadeOut(_fadeOutTime);
            yield return SceneManager.LoadSceneAsync(_sceneToLoad);

            Portal otherPortal = GetOtherPortal();
            UpdatePlayer(otherPortal);

            yield return new WaitForSeconds(_fadeWaitTime);
            yield return fader.Cor_FadeIn(_fadeInTime);

            Destroy(this.gameObject);
        }

        private void UpdatePlayer(Portal otherPortal)
        {
            GameObject player = GameObject.FindWithTag("Player");
            player.GetComponent<NavMeshAgent>().Warp(otherPortal._spawnPoint.position);
            player.transform.rotation = otherPortal._spawnPoint.rotation;
        }

        private Portal GetOtherPortal()
        {
            foreach (var portal in FindObjectsOfType<Portal>())
            {
                if (portal == this || portal._destination != _destination)
                    continue;

                return portal;
            }

            return null;
        }
    }
}