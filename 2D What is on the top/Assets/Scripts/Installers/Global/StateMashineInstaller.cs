using GameSM;
using Zenject;

namespace Installers
{
    public class StateMashineInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            BindGameStateMachine();
        }

        private void BindGameStateMachine()
        {
            var gameStateMachine = new GameStateMachine(GameStateType.GameMenu);
            
            Container.Bind<IGameCurrentState>().To<GameStateMachine>().FromInstance(gameStateMachine).AsSingle();

        }
    }
}