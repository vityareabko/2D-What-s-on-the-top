using TMPro;
using UI.MVP;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public interface IGameScreenDefeatView: IView<IGameScreenDefeatPresenter>
    {
    }

    public class GameScreenDefeatView : BaseScreenView, IGameScreenDefeatView
    {
        public override ScreenType ScreenType { get; } = ScreenType.GameScreenDefeat;

        [SerializeField] private TMP_Text _amountConis;
        [SerializeField] private Button _x2WatchAdsButton;
        [SerializeField] private Button _startAgainButton;
        [SerializeField] private Button _homeButton;

        public IGameScreenDefeatPresenter Presentor { get; private set; }
        
        protected override void OnAwake()
        {
            base.OnAwake();
            _x2WatchAdsButton.onClick.AddListener(OnX2WatchAdsButton);
            _startAgainButton.onClick.AddListener(OnStartAgainButton);
            _homeButton.onClick.AddListener(OnHomeButton);
        }
        
        public void InitPresentor(IGameScreenDefeatPresenter presentor) => Presentor = presentor;

        public void SetCoins(int amount) => _amountConis.text = amount.ToString();

        private void OnStartAgainButton() => Presentor.OnAgainButtonClicked();
       
        private void OnHomeButton() => Presentor.OnHomeButtonClicked();
        
        private void OnX2WatchAdsButton() => Presentor.OnX2RewardWatchButton();
        

    }
}