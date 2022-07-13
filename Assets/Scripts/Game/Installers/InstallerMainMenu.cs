using UI;
using UnityEngine;
using Zenject;

namespace Installer
{
    public class InstallerMainMenu : MonoInstaller
    {
        [Header("References")]
        [SerializeField] GameObject mainMenuUI;
        [SerializeField] GameObject optionsUI;
        [SerializeField] GameObject loadingUI;

        public override void InstallBindings()
        {
            Container.Bind<MainMenuUI>().FromComponentOn(mainMenuUI).AsSingle();
            Container.Bind<OptionsUI>().FromComponentOn(optionsUI).AsSingle();
            Container.Bind<LoadingUI>().FromComponentOn(loadingUI).AsSingle();
        }
    }
}