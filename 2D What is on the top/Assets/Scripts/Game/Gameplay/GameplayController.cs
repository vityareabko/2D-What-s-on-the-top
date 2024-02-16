using UnityEngine;

namespace Game.Gameplay
{
    public class GameplayController : MonoBehaviour
    {
        private void OnEnable()
        {
            EventAggregator.Subscribe<PauseGameEventHandler>(OnPauseGame);
            EventAggregator.Subscribe<ResumeGameEventHandler>(OnResumeGame);
        }

        private void OnDisable()
        {
            EventAggregator.Unsubscribe<PauseGameEventHandler>(OnPauseGame);
            EventAggregator.Unsubscribe<ResumeGameEventHandler>(OnResumeGame);
        }

        public void OnPauseGame(object sender, PauseGameEventHandler eventdata)
        {
            Time.timeScale = 0;
            EventAggregator.Post(this, new GameIsOnPausedEvent(){ IsOnPause = true });
        }

        public void OnResumeGame(object sender, ResumeGameEventHandler eventData)
        {
            Time.timeScale = 1;
            EventAggregator.Post(this, new GameIsOnPausedEvent(){ IsOnPause = false});
        }
    }
}