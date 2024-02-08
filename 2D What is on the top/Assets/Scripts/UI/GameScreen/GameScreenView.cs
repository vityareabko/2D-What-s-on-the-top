using TMPro;
using UI.MVP;
using UnityEngine;

namespace UI
{
    public interface ISreenView : IView
    {
        public void SetScore(int score);
    }

    public class GameScreenView : BaseScreenView, ISreenView
    {
        public override ScreenType ScreenType { get; } = ScreenType.GameScreen;

        [SerializeField] private TMP_Text _scoreText;

        private string _scoreFormat;

        public void SetScore(int score)
        {
            _scoreText.text = string.Format(_scoreFormat, score);
        }

        protected override void OnAwake()
        {
            base.OnAwake();
            _scoreFormat = _scoreText.text;
        }
    }
}