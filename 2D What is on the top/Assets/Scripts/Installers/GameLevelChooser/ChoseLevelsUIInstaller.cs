using UI.GameLevelChooser.ChooserLevel;
using UnityEngine;
using Zenject;

namespace Installers.GameLevelChooser
{
    public class ChoseLevelsUIInstaller : MonoInstaller
    {
        [SerializeField] private GameLevelChooserView _gameLevelChooser;
        [SerializeField] private Transform _parent;
        
        public override void InstallBindings()
        {
            BindLevelChoserMVP();
        }

        private void BindLevelChoserMVP()
        {
            Container.BindInterfacesAndSelfTo<GameLevelChooserModel>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<GameLevelChooserView>().FromComponentInNewPrefab(_gameLevelChooser).UnderTransform(_parent).AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<GameLevelChooserPresenter>().AsSingle().NonLazy();

        }
    }
}