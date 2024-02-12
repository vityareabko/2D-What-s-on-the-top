using UI.MVP;
using UnityEngine;
using Zenject;

namespace UI
{
    public interface IGameScreenDefeatPresenter : IPresenter<IGameScreenDefeatView>
    {
        public void OnHomeButtonClicked();
        public void OnAgainButtonClicked();
        public void OnX2RewardWatchButton();
    }

    public class GameScreenDefeatPresenter : IGameScreenDefeatPresenter
    {
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
        
        public void Hide() => View.Hide();
        
        public void OnHomeButtonClicked()
        {
           Debug.Log("home button clicked");
           
           // к главному экранну 
           
           Hide();
        }

        public void OnAgainButtonClicked()
        {
            Debug.Log("again button clicked");
            
            // начать игру заново
            
            Hide();
        }

        public void OnX2RewardWatchButton()
        {
            Debug.Log("x2 reward ads button clicked");
            
            // посмотреть рекламы и дать выйграш x2
            
            Hide();
        }
    }
}