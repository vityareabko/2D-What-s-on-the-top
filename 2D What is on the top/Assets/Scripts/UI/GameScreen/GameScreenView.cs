using TMPro;
using UI.MVP;
using UnityEngine;

namespace UI
{
    public interface IGameSreenView : IView<IGameScreenPresenter>
    {
        public void SetScore(int score);
    }

    public class GameScreenView : BaseScreenView, IGameSreenView
    {
        [SerializeField] private TMP_Text _scoreText;
        
        public override ScreenType ScreenType { get; } = ScreenType.GameScreen;
        
        public IGameScreenPresenter Presentor { get; private set; }

        private string _scoreFormat;

        public void SetScore(int score)
        {
            Debug.Log(score);
            _scoreText.text = string.Format(_scoreFormat, score);
        }

        protected override void OnAwake()
        {
            base.OnAwake();
            _scoreFormat = _scoreText.text;
        }
        
        public void InitPresentor(IGameScreenPresenter presentor)
        {
            Presentor = presentor;
        }
    }
}