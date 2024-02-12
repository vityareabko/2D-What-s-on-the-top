using UI.MVP;
using UnityEngine;
using Zenject;

namespace UI.GameScreenLevelWinn
{
    public interface IGameScreenLevelWinPresenter : IPresenter<IGameScrenLevelWinModel, IGameScreenLevelWinView>
    {
        public void OnClickClaimButton();
        public void OnClickClaimX2AdsButton();
    }

    public class GameScreenLevelWinPresenter : IGameScreenLevelWinPresenter
    {
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

        public void OnClickClaimButton()
        {
            // принимаем награду сохраняем и переходим на главный экран
            Debug.Log("Claim rewards");
            
            View.Hide();
        }

        public void OnClickClaimX2AdsButton()
        {
            // попказываем рекламу проверяем если рекламу посмотрели полностью и даем X2 награду, сохраняем, и переходим на главный экран
            Debug.Log("Claim X2 rewards");
            
            View.Hide();
        }
    }
}