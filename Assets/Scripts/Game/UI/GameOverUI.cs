using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace UI
{
    public class GameOverUI : MonoUI
    {
        [Header("Resources")]
        [SerializeField] Player.PlayerData playerData;

        private LoadingUI _loadingUI;
        private float _timer;

        private void Update()
        {
            if (IsActive())
                return;

            _timer += Time.deltaTime;

            if (_timer > 0.2f)
            {
                if(playerData.health <= 0)
                    EnterState();
                _timer = 0;
            }
        }


        [Inject]
        private void Construct(LoadingUI loadingUI)
        {
            _loadingUI = loadingUI;
        }

        #region OnClick
        public void OnClickRestart() => _loadingUI.LoadSceneAsync(1);
        #endregion
    }
}
