using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI
{
    public class GameOverUI : MonoUI
    {
        [Header("Resources")]
        [SerializeField] Player.PlayerData playerData;

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

        #region OnClick
        public void OnClickRestart()
        {
            SceneManager.LoadScene(1);
        }
        #endregion
    }
}
