using System;

namespace GameSM
{
    public enum GameStateType
    {
        GameMenu,
        GamePlay,
        LoseGame,
        WinGame,
    }

    public interface IGameCurrentState
    {
        public GameStateType CurrentState { get; }
    }

    public class GameStateMachine : IGameCurrentState, IDisposable
    {
        public GameStateType CurrentState { get; private set; }

        public GameStateMachine(GameStateType currentState)
        {
            CurrentState = currentState;
            
            EventAggregator.Subscribe<SwitchGameStateToPlayGameEvent>(OnSwitchToPlayGameState);
            EventAggregator.Subscribe<SwitchGameStateToLoseGameEvent>(OnSwitchToLoseGameState);
            EventAggregator.Subscribe<SwitchGameStateToWinGameEvent>(OnSwitchToWinGameState);
            EventAggregator.Subscribe<SwitchGameStateToMainMenuGameEvent>(OnSwitchToMainMenuGameState);
        }
        
        public void Dispose()
        {
            EventAggregator.Unsubscribe<SwitchGameStateToPlayGameEvent>(OnSwitchToPlayGameState);
            EventAggregator.Unsubscribe<SwitchGameStateToLoseGameEvent>(OnSwitchToLoseGameState);
            EventAggregator.Unsubscribe<SwitchGameStateToWinGameEvent>(OnSwitchToWinGameState);
            EventAggregator.Unsubscribe<SwitchGameStateToMainMenuGameEvent>(OnSwitchToMainMenuGameState);
        }

        
        private void SwitchState(GameStateType state)
        {

            switch (state)
            {
                case GameStateType.GameMenu:
                    // Какие-то действия перед смены состояния
                    
                    // должеы вернуть камеру на main Menu position за 1 секунды плавно 
                    // должен взять игрока и переместить на main Menu Postion
                    // все
                    
                    CurrentState = GameStateType.GameMenu;
                    break;
                case GameStateType.GamePlay:
                    
                    // должен показать экран HUD но сначала инициализировать его 
                    
                    CurrentState = GameStateType.GamePlay;
                    break;
                case GameStateType.LoseGame:
                    
                    CurrentState = GameStateType.LoseGame;
                    break;
                case GameStateType.WinGame:
                    CurrentState = GameStateType.WinGame;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(state), state, null);
            }
        }
        
        private void OnSwitchToPlayGameState(object sender, SwitchGameStateToPlayGameEvent evenData) => SwitchState(GameStateType.GamePlay);
        private void OnSwitchToMainMenuGameState(object sender, SwitchGameStateToMainMenuGameEvent evenData) => SwitchState(GameStateType.GameMenu);
        private void OnSwitchToWinGameState(object sender, SwitchGameStateToWinGameEvent evenData) => SwitchState(GameStateType.WinGame);
        private void OnSwitchToLoseGameState(object sender, SwitchGameStateToLoseGameEvent evenData) => SwitchState(GameStateType.LoseGame);
        
            
        // private void SwitchGameStateEvent(GameStateType state)
        // {
        //     UpdateState(state);
        // }
        //
        // private void OnGameMenuState()
        // {
        //     // check to startButton pressed - это реакция на события (нажатия кнопки в менюшке для из меню сост. в состояние игры)
        //     
        //     SwitchGameStateEvent(GameStateType.GamePlay);
        // }
        //
        // private void OnGamePlayState()
        // {
        //     // check to Win or Lose Game - это метод реакция на проиграш или выйграш игры в зависимости от выйграша или проиграша переключаем состояние
        //     
        //     // if (win)
        //     SwitchGameStateEvent(GameStateType.WinGame);
        //     // else 
        //     SwitchGameStateEvent(GameStateType.LoseGame);
        // }
        //
        // private void OnGameWinState()
        // {
        //     // checking for press some button if it is - это реакция на перехож в состояния меню - а имено если кнопка (claim, x2Reward) переключаем в состояния меню 
        //     SwitchGameStateEvent(GameStateType.GameMenu);
        // }
        //
        // private void OnGameLoseState()
        // {
        //     // checking for press some button if it is - это реакция на перехож в состояния меню - а имено если кнопка (claim, x2Reward) переключаем в состояния меню 
        //     SwitchGameStateEvent(GameStateType.GameMenu);
        // }
        
    }
}