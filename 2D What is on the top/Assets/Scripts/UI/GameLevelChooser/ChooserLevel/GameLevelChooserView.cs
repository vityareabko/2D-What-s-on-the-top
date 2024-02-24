using System;
using UI.MVP;
using UnityEngine;
using UnityEngine.UI;

namespace UI.GameLevelChooser.ChooserLevel
{
    public interface IGameLevelChooserView : IView<IGameLevelChooserPresenter>
    {
    }

    public class GameLevelChooserView : BaseScreenView, IGameLevelChooserView
    {
        public override ScreenType ScreenType { get; } = ScreenType.ChooserLevel;

        [SerializeField] private Button _lvl1Button;
        [SerializeField] private Button _lvl2Button;
        
        public IGameLevelChooserPresenter Presentor { get; private set; }

        public void InitPresentor(IGameLevelChooserPresenter presentor) => Presentor = presentor;

        private void OnEnable()
        {
            _lvl1Button.onClick.AddListener(OnLevel1ButtonCliked);  
            _lvl2Button.onClick.AddListener(OnLevel2ButtonCliked);  
        }


        private void OnDisable()
        {
            _lvl1Button.onClick.RemoveListener(OnLevel1ButtonCliked);  
            _lvl2Button.onClick.RemoveListener(OnLevel2ButtonCliked);  
        }

        private void OnLevel1ButtonCliked() { Presentor.OnLevelSelected(LevelType.Level1);}
        private void OnLevel2ButtonCliked() { Presentor.OnLevelSelected(LevelType.TestLevel);}
    }
}