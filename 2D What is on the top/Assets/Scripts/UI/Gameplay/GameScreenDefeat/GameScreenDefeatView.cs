using System.Collections.Generic;
using System.Linq;
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
        [SerializeField] private TMP_Text _descriptionText;
        
        [SerializeField] private Button _x2WatchAdsButton;
        [SerializeField] private Button _homeButton;
        
        public IGameScreenDefeatPresenter Presentor { get; private set; }
        
        protected override void OnAwake()
        {
            base.OnAwake();
            _descriptionText.text = SelectRandomPhrace();
            
            _x2WatchAdsButton.onClick.AddListener(OnX2WatchAdsButton);
            _homeButton.onClick.AddListener(OnHomeButton);
        }
        
        public void InitPresentor(IGameScreenDefeatPresenter presentor) => Presentor = presentor;

        public void SetCoins(int amount) => _amountConis.text = amount.ToString();
       
        private void OnHomeButton() => Presentor.OnHomeButtonClicked();
        
        private void OnX2WatchAdsButton() => Presentor.OnX2RewardWatchButton();

        private string SelectRandomPhrace()
        {
            string[] phrases = new[]
            {
                "Looks like the well was too deep this time. Try again?",
                "Falling down is just a step back, not the end. Climb up!",
                "You hit rock bottom, but the only way now is up!",
                "The well is deep, but your determination is deeper. Go again!",
                "Don't let the darkness hold you back. The sky is still above!",
                "Every fall is a lesson. What did you learn this time?",
                "You might have slipped, but don't let it dampen your spirit!",
                "The climb is tough, but so are you. Ready for another go?",
                "This well can't contain you forever. Break free!",
                "Gravity might have won this round, but the game isn't over yet."
            };

            return phrases[Random.Range(0, phrases.Length - 1)];
        }

    }
}