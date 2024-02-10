using System;
using UI.MVP;
using Zenject;

namespace UI
{
    public interface IGameScreenPresenter : IPresenter<IGameScreenModel, IGameSreenView> { }

    public class GameScreenPresenter : IGameScreenPresenter, IDisposable
    {
        public IGameSreenView View { get; private set; }
        public IGameScreenModel Model { get; private set; }

        private CharacterData _characterData;

        private bool _isInit = false;

        [Inject] public GameScreenPresenter(IGameScreenModel model, IGameSreenView view, CharacterData characterData)
        {
            Model = model;
            View = view;
            _characterData = characterData;
            
            Init();
        }

        public void Dispose()
        {
            Model.HightScoreChange -= OnHightScoreChange;
            Model.StaminaChange -= OnStaminaChange;
        }

        public void Init()
        {
            if (_isInit)
                return;

            _isInit = true;

            View.Initialize(_characterData.StaminaData);

            Model.HightScoreChange += OnHightScoreChange;
            Model.StaminaChange += OnStaminaChange;
        }


        public void Show() { PrepareData(); View.Show(); }
        public void Hide() => View.Hide();

        public void UpdateHeightScore(int score) => Model.Score = score;

        public void UpdateStamina(float stamina) => Model.Stamina = stamina;

        private void PrepareData()
        {
            View.SetHightScore(Model.Score);
            View.SetStaminaValue(Model.Stamina);
        }

        private void OnStaminaChange(float stamina)
        {
            View.SetStaminaValue(stamina);
        }
        
        private void OnHightScoreChange(int score)
        {
            View.SetHightScore(score);
        }
    }
}