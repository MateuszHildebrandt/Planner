using Zenject;

namespace UI
{
    public class GameMenuUI : MonoUI
    {
        private InputActions _inputActions;
        private HeadUpDisplayUI _headUpDisplayUI;
        private LoadingUI _loadingUI;

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
        private void Construct(InputActions inputActions, HeadUpDisplayUI headUpDisplayUI, LoadingUI loadingUI)
        {
            _inputActions = inputActions;
            _headUpDisplayUI = headUpDisplayUI;
            _loadingUI = loadingUI;
        }


        private void Toggle()
        {
            if (IsActive())
                _headUpDisplayUI.EnterState();
            else
                EnterState();
        }

        #region OnClick
        public void OnClickLoad() => _loadingUI.LoadSceneAsync(1);

        public void OnClickExit() => _loadingUI.LoadSceneAsync(0);
        #endregion
    }
}
