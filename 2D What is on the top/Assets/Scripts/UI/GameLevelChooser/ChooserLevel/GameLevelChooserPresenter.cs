using System;
using UI.MVP;

namespace UI.GameLevelChooser.ChooserLevel
{
    public interface IGameLevelChooserPresenter : IPresenter<IGameLevelChooserModel, IGameLevelChooserView>
    {
        public event Action<LevelType> OnPlayButtonClicked;
        public void OnLevelSelected(LevelType type);
    }

    public class GameLevelChooserPresenter : IGameLevelChooserPresenter
    {
        public event Action<LevelType> OnPlayButtonClicked;
        public IGameLevelChooserModel Model { get; }
        public IGameLevelChooserView View { get; }

        private LevelDatabases _levelDatabases;
        private SceneLoadMediator _sceneLoader;
        
        public bool _isInit;
        
        public GameLevelChooserPresenter(IGameLevelChooserModel model, IGameLevelChooserView view, LevelDatabases levelDatabases, SceneLoadMediator sceneLoadMediator)
        {
            Model = model;
            View = view;
            
            Init();
            
            _levelDatabases = levelDatabases;
            _sceneLoader = sceneLoadMediator;
        }
        
        
        public void Show() => View.Show();
        public void Hide() => View.Hide();

        public void Init()
        {
            if (_isInit)
                return;
            
            _isInit = true;
            View.InitPresentor(this);
        }

        public void OnLevelSelected(LevelType type)
        {
            //Через Action
            OnPlayButtonClicked?.Invoke(type);
            
            // я могу через EventAggregator передать тип конфигурации, и уже там разбиратся
            // также могу сделать как и везде в MVP через события и в Controller уже разбиратся
            // Этот вариант не нарушает MVP но также он должен знать об _levelDatabase что не очень
            // var a = new GoToGameplaySceneEvent();
            // a.LevelConfig = _levelDatabases.LevelConfigs[type];
            // EventAggregator.Post(this, a);
            
            // а могу здесь разобратся - дать доступ к LevelDatabase найти нужную конфигурацию через тип, прокинуть SceneLoaderMediator и открыть сцену передав ее конфигурацию
            // этот вариант прекрасен но он нарушает как по мне принцып MVP так как это уже не относится к загрузке сцен или тому подобное
            // if (_levelDatabases.LevelConfigs.ContainsKey(type) == false)
            //     throw new ArgumentException($"level {type} - couldn't founded");
            //
            // var levelConfig = _levelDatabases.LevelConfigs[type];
            // _sceneLoader.GoToGameplay(levelConfig);
        }
    }
}

