using UnityEngine;
using Zenject;

namespace Installers.GameplayInstallers
{
    public class PlayerInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            BindPayerStamin();
        }

        private void BindPayerStamin()
        {
            Container.Bind<Stamina>().AsSingle();
        }
    }
}