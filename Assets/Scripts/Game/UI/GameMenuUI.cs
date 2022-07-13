using UnityEngine.SceneManagement;
using Zenject;

namespace UI
{
    public class GameMenuUI : MonoUI
    {
        private InputActions _inputActions;
        private HeadUpDisplayUI _headUpDisplayUI;

        private void Start()
        {
            _inputActions.UI.Enable();
            _inputActions.UI.Escape.performed += (_) => Toggle();
        }

        private void OnDestroy()
        {
            _inputActions.Dispose();
        }

        [Inject]
        private void Construct(InputActions inputActions, HeadUpDisplayUI headUpDisplayUI)
        {
            _inputActions = inputActions;
            _headUpDisplayUI = headUpDisplayUI;
        }


        private void Toggle()
        {
            if (IsActive())
                _headUpDisplayUI.EnterState();
            else
                EnterState();
        }

        #region OnClick
        public void OnClickLoad()
        {
            SceneManager.LoadScene(1);
        }

        public void OnClickExit()
        {
            SceneManager.LoadScene(0);
        }
        #endregion
    }
}
