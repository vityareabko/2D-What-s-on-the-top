using GameSM;

namespace Systems.SceneSystem
{
    public interface ISceneSystem
    {
        // public void ReloadScene();
        // public void ReloadSceneByStateGame(GameStateType state);
    }

    public class SceneSystem : ISceneSystem
    {
        // public void ReloadSceneByStateGame(GameStateType state)
        // {
        //     SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        //
        //     switch (state)
        //     {
        //         case GameStateType.GameMenu:
        //             EventAggregator.Post(this, new SwitchGameStateToMainMenuGameEvent());
        //             break;
        //         case GameStateType.GamePlay:
        //             EventAggregator.Post(this, new SwitchGameStateToPlayGameEvent());
        //             break;
        //         case GameStateType.LoseGame:
        //             EventAggregator.Post(this, new SwitchGameStateToLoseGameEvent());
        //             break;
        //         case GameStateType.WinGame:
        //             EventAggregator.Post(this, new SwitchGameStateToWinGameEvent());
        //             break;
        //         default:
        //             throw new ArgumentOutOfRangeException(nameof(state), state, null);
        //     }
        // }
    }
}