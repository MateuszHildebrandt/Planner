using UnityEngine;
using System.Collections;
using System;
using Zenject;

namespace UI
{
    public class LogoUI : MonoUI
    {
        [Header("References")]
        [SerializeField] AudioSource audioSource;
        [Header("Resources")]
        [SerializeField] AudioClip sound;
        [Header("Settings")]
        [SerializeField] int logoTime = 1;

        private MainMenuUI _mainMenuUI;

        [Inject]
        private void Constructor(MainMenuUI mainMenuUI)
        {
            _mainMenuUI = mainMenuUI;
        }

        private IEnumerator WhaitFor(float time, Action onDone)
        {
            yield return new WaitForSeconds(time);
            onDone?.Invoke();
        }

        #region StateMachine
        public override void OnEnter()
        {
            base.OnEnter();

            audioSource.PlayOneShot(sound);

            StopAllCoroutines();
            StartCoroutine(WhaitFor(logoTime, () => _mainMenuUI.EnterState()));
        }
        #endregion
    }
}
