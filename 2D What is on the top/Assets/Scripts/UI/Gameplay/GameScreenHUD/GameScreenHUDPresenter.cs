using System;
using UI.MVP;
using Zenject;

namespace UI
{
    public interface IGameScreenPresenter : IPresenter<IGameScreenModel, IGameSreenView>
    {
        public event Action OnPauseClicked; 
        public void OnPauseButtonClicked();
        public void SetAmountCoins(int value);
    }

    public class GameScreenHUDPresenter : IGameScreenPresenter, IDisposable
    {
        public event Action OnPauseClicked;
        
        public IGameSreenView View { get; private set; }
        public IGameScreenModel Model { get; private set; }
        
        private PlayerConfig playerConfig;
        
        private bool _isInit = false;

        public GameScreenHUDPresenter(
            IGameScreenModel model, 
            IGameSreenView view, 
            PlayerConfig playerConfig)
        {
            Model = model;
            View = view;
            this.playerConfig = playerConfig;
            
            Init();
        }

        public void Init()
        {
            if (_isInit)
                return;

            _isInit = true;

            View.Initialize(playerConfig.StaminaData);
            View.InitPresentor(this);

            
            Model.HightScoreChange += OnHightScoreChange;
            Model.StaminaChange += OnStaminaChange;
            EventAggregator.Subscribe<PopupTextDrainStaminEvent>(OnSpawnPopupTextStaminaDrain);
        }

        public void Dispose()
        {
            Model.HightScoreChange -= OnHightScoreChange;
            Model.StaminaChange -= OnStaminaChange;
            EventAggregator.Unsubscribe<PopupTextDrainStaminEvent>(OnSpawnPopupTextStaminaDrain);
        }

        public void Show()
        {
            PrepareData();
            View.Show();
        }
        
        public void Hide(System.Action callBack = null) => View.Hide(callBack);

        public void UpdateHeightScore(int score) => Model.Score = score;

        public void UpdateStamina(float stamina) => Model.Stamina = stamina;

        private void PrepareData()
        {
            View.SetHightScore(Model.Score);
            View.SetStaminaValue(Model.Stamina);
        }
        
        public void SetAmountCoins(int value) => View.SetAmountCoins(value);
        
        public void OnPauseButtonClicked() => OnPauseClicked?.Invoke(); 

        private void OnStaminaChange(float stamina) => View.SetStaminaValue(stamina);
        
        private void OnHightScoreChange(int score) => View.SetHightScore(score);
        
        private void OnSpawnPopupTextStaminaDrain(object sender, PopupTextDrainStaminEvent eventData) =>
            View.SpawnPopupTextDrainStamina(eventData.DrainAmount);
        
    }
}