using Services;
using UnityEngine;
using Zenject;

namespace Installers
{
    public class ServiceInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            BindPause();
        }

        private void BindPause()
        {
            Container.Bind<IPause>().To<PauseHandler>().AsSingle();
        }
    }
}