using UI.MVP;
using Zenject;

namespace UI
{
    public interface IGameScreenDefeatPresenter : IPresenter<IGameScreenDefeatView>
    {
        public event System.Action HomeButtonCliked; 
        public event System.Action OnX2RewardButtonCliked; 
        public void OnHomeButtonClicked();
        public void OnX2RewardWatchButton();
    }

    public class GameScreenDefeatPresenter : IGameScreenDefeatPresenter
    {
        public event System.Action HomeButtonCliked;
        public event System.Action OnX2RewardButtonCliked;
        
        public IGameScreenDefeatView View { get; }

        public bool _isInit = false;

        [Inject] public GameScreenDefeatPresenter(IGameScreenDefeatView view)
        {
            View = view;
            Init();
        }

        public void Init()
        {
            if (_isInit) return;

            _isInit = true;
            
            View.InitPresentor(this);
        }
        
        public void Show() => View.Show();
        
        public void Hide(System.Action callBack = null) => View.Hide(callBack);

        public void OnHomeButtonClicked() => HomeButtonCliked?.Invoke();
        public void OnX2RewardWatchButton() => OnX2RewardButtonCliked?.Invoke();
        
    }
}