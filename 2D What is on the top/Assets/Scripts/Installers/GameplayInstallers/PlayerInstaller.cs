using UnityEngine;
using Zenject;

namespace Installers.GameplayInstallers
{
    public class PlayerInstaller : MonoInstaller
    {
        [SerializeField] private PlayerController _playerController;
        
        public override void InstallBindings()
        {
            BindPayerStamin();
            BindCharacterController();
        }

        private void BindPayerStamin()
        {
            Container.Bind<Stamina>().AsSingle();
        }

        private void BindCharacterController()
        {
            Container.Bind<ICharacterEvents>().FromInstance(_playerController).AsSingle();
        }
    }
}