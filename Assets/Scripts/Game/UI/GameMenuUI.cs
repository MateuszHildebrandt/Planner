using UnityEngine.SceneManagement;
using Zenject;

namespace UI
{
    public class GameMenuUI : MonoUI
    {
        private InputActions _inputActions;
        private HeadUpDisplayUI _headUpDisplayUI;
        private bool _isActive;

        private void Start()
        {
            _inputActions = new InputActions();
            _inputActions.UI.Enable();
            _inputActions.UI.Escape.performed += (_) => Toggle();
        }

        private void OnDestroy()
        {
            _inputActions.Dispose();
        }

        [Inject]
        private void Construct(HeadUpDisplayUI headUpDisplayUI)
        {
            _headUpDisplayUI = headUpDisplayUI;
        }

        private void Toggle()
        {
            _isActive = !_isActive;

            if (_isActive)
                EnterState();
            else
                _headUpDisplayUI.EnterState();
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
