using System;
using UI.MVP;
using UnityEngine;
using UnityEngine.UI;

namespace UI.GameScreenLevelWinn
{
    public interface IGameScreenLevelWinView : IView<IGameScreenLevelWinPresenter>
    {
    }

    public class GameScreenLevelWinView : BaseScreenView, IGameScreenLevelWinView
    {
        public override ScreenType ScreenType { get; } = ScreenType.GameScreenLevelWin;
        
        [SerializeField] private Button _claimButton;
        [SerializeField] private Button _x2AdsClaimButton;
        
        public IGameScreenLevelWinPresenter Presentor { get; private set; }
        
        public void InitPresentor(IGameScreenLevelWinPresenter presentor) => Presentor = presentor;

        private void OnEnable()
        {
            _claimButton.onClick.AddListener(OnClickClaimButton);
            _x2AdsClaimButton.onClick.AddListener(OnClickX2ClaimButton);
        }

        private void OnDisable()
        {
            _claimButton.onClick.RemoveListener(OnClickClaimButton);
            _x2AdsClaimButton.onClick.RemoveListener(OnClickX2ClaimButton);
        }

        private void OnClickClaimButton() => Presentor.OnClickClaimButton();
        private void OnClickX2ClaimButton() => Presentor.OnClickClaimX2AdsButton();
    }
}