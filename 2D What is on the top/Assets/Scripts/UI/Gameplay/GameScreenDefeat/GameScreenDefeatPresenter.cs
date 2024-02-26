using System;
using UI.MVP;
using Unity.VisualScripting;
using UnityEngine;
using Zenject;

namespace UI
{
    public interface IGameScreenDefeatPresenter : IPresenter<IGameScreenDefeatView>
    {
        public event System.Action HomeButtonCliked; 
        public event System.Action RestartLevelButtonCliked; 
        public event System.Action OnX2RewardButtonCliked; 
        public void OnHomeButtonClicked();
        public void OnAgainButtonClicked();
        public void OnX2RewardWatchButton();
    }

    public class GameScreenDefeatPresenter : IGameScreenDefeatPresenter
    {
        public event Action HomeButtonCliked;
        public event Action RestartLevelButtonCliked;
        public event Action OnX2RewardButtonCliked;
        
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

        public void OnHomeButtonClicked() => HomeButtonCliked?.Invoke();
        public void OnAgainButtonClicked() => RestartLevelButtonCliked?.Invoke();
        public void OnX2RewardWatchButton() => OnX2RewardButtonCliked?.Invoke();
        
    }
}