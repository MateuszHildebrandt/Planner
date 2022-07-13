using UnityEngine;
using Zenject;

namespace Game
{
    public class InstallerGame : MonoInstaller
    {
        [Header("References")]
        [SerializeField] BulletsPool magicBullets;

        public override void InstallBindings()
        {
            Container.Bind<InputActions>().AsSingle();
            Container.Bind<BulletsPool>().FromComponentOn(magicBullets.gameObject).AsSingle();
        }
    }
}
