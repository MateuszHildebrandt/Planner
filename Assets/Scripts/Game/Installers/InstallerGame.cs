using UnityEngine;
using Zenject;

namespace Game
{
    public class InstallerGame : MonoInstaller
    {
        [SerializeField] BulletsPool magicBullets;

        public override void InstallBindings()
        {
            Container.Bind<BulletsPool>().FromComponentOn(magicBullets.gameObject).AsSingle();
        }
    }
}
