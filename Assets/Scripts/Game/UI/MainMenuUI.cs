using Tools;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Zenject;

namespace UI
{
    public class MainMenuUI : MonoUI
    {
        [Header("References")]
        [SerializeField] Button continueButton;
        [Header("Resources")]
        [SerializeField] AudioSource audioSource;

        private bool _firstVisit = true;
        private OptionsUI _optionsUI;

        [Inject]
        private void Constructor(OptionsUI optionsUI)
        {
            _optionsUI = optionsUI;
        }

        #region OnClick
        public void OnClickNewGame()
        {
            File.Delete(FilesManager.PlayerDataPath);
            SceneManager.LoadScene(1);
        }

        public void OnClickContinue()
        {
            SceneManager.LoadScene(1);
        }

        public void OnClickOptions() => _optionsUI.EnterState();
        public void OnClickExit() => Application.Quit();
        #endregion

        #region StateMachine
        public override void OnEnter()
        {
            base.OnEnter();

            if (_firstVisit)
            {
                audioSource.Play();
                continueButton.interactable = File.Exists(FilesManager.PlayerDataPath);
                _firstVisit = false;
            }
        }
        #endregion
    }
}
