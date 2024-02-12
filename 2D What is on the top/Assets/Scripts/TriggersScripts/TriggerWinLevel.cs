using System;
using Helper;
using UnityEngine;

namespace TriggersScripts
{
    public interface ILevelWin
    {
        public event Action LevelWin;
    }

    public class TriggerWinLevel : MonoBehaviour, ILevelWin
    {
        public event Action LevelWin;
        
        // не надо инжектить лучше сделать какой-то GameLevelController где будем там подписыватся на этот acction и там реагировать
        
        private void OnTriggerEnter2D(Collider2D colider)
        {
            if (colider.CompareTag(ConstTags.Player))
                LevelWin?.Invoke();
        }
    }
}
