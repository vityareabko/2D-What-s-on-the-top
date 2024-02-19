using Zenject;

namespace Installers.GameplayInstallers
{
    public class GameplayLevelInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            BindPayerStamin();
        }

        private void BindPayerStamin() => Container.Bind<Stamina>().AsSingle();
    }
}