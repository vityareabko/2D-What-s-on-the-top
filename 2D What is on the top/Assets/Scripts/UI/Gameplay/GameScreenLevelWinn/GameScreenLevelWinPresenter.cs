using UI.MVP;
using Zenject;

namespace UI.GameScreenLevelWinn
{
    public interface IGameScreenLevelWinPresenter : IPresenter<IGameScrenLevelWinModel, IGameScreenLevelWinView>
    {
        public event System.Action ClaimButtonClicked; 
        public event System.Action X2RewardButtonClicked; 
        public void OnClickClaimButton();
        public void OnClickClaimX2AdsButton();
    }

    public class GameScreenLevelWinPresenter : IGameScreenLevelWinPresenter
    {
        public event System.Action ClaimButtonClicked;
        public event System.Action X2RewardButtonClicked;
        
        public IGameScrenLevelWinModel Model { get; }
        public IGameScreenLevelWinView View { get; }

        private bool _isInit = false;
        
        [Inject] public GameScreenLevelWinPresenter(IGameScreenLevelWinView view, IGameScrenLevelWinModel model)
        {
            View = view;
            Model = model;
            
            Init();
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

        public void OnClickClaimButton() => ClaimButtonClicked?.Invoke();

        public void OnClickClaimX2AdsButton() => X2RewardButtonClicked?.Invoke();
    }
}