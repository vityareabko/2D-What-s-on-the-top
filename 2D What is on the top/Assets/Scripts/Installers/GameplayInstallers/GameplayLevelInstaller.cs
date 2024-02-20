using Score;
using Zenject;

namespace Installers.GameplayInstallers
{
    public class GameplayLevelInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            BindPayerStamin();
            BindScoreController();
        }

        private void BindPayerStamin() => Container.Bind<Stamina>().AsSingle();
        
        // private void BindScoreController() => Container.Bind<ScoreController>().FromInstance(instace).AsSingle()

        private void BindScoreController() => Container.Bind<ScoreController>().AsSingle().NonLazy();
    }
}