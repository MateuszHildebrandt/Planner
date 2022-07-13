using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI
{
    public class LoadingUI : MonoUI
    {
        internal void LoadSceneAsync(int index)
        {
            EnterState();
            SceneManager.LoadSceneAsync(index);
        }
    }
}
