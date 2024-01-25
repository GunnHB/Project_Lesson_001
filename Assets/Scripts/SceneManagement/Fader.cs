using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.SceneManagement
{
    public class Fader : MonoBehaviour
    {
        private CanvasGroup _canvasGroup;

        private Coroutine _fadeOutCoroutine;

        private void Start()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
        }

        public IEnumerator Cor_FadeOutIn()
        {
            yield return Cor_FadeOut(2f);
            print("Faded out");
            yield return Cor_FadeIn(2f);
            print("Faded in");
        }

        public IEnumerator Cor_FadeIn(float time)
        {
            while (_canvasGroup.alpha > 0f)
            {
                _canvasGroup.alpha -= Time.deltaTime / time;
                yield return null;
            }
        }

        public IEnumerator Cor_FadeOut(float time)
        {
            while (_canvasGroup.alpha < 1f)
            {
                _canvasGroup.alpha += Time.deltaTime / time;
                yield return null;
            }
        }
    }
}