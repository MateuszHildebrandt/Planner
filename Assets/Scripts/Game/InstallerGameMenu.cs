using UI;
using UnityEngine;
using Zenject;

namespace Installer
{
    public class InstallerGameMenu : MonoInstaller
    {
        [Header("References")]
        [SerializeField] GameObject headUpDisplayUI;

        public override void InstallBindings()
        {
            //Container.Bind<HeadUpDisplayUI>().FromComponentInHierarchy().AsSingle();
            //Container.Bind<HeadUpDisplayUI>().FromComponentInChildren().AsSingle();
            Container.Bind<HeadUpDisplayUI>().FromComponentOn(headUpDisplayUI).AsSingle();
        }
    }
}