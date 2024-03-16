using System;
using Obstacles;
using PersistentData;
using UnityEngine;
using Zenject;

public class TriggerPlayerTransionToOtherLevel : MonoBehaviour
{
    public LevelType Type;
    
    private IPlayer _player;
    private LevelsDB _levelsDB;
    private IPersistentPlayerData _persistentPlayerData;
    
    [Inject] private void Construct(IPlayer player, LevelsDB levelsDB, IPersistentPlayerData persistentPlayerData)
    {
        _player = player;
        _levelsDB = levelsDB;
        _persistentPlayerData = persistentPlayerData;
    }

    private void Update()
    {
        if (_player.Transform.position.y > transform.position.y)
            GetComponent<BoxCollider2D>().isTrigger = false;
    }

    private void OnTriggerExit2D(Collider2D colider)
    {
        if (colider.CompareTag(ConstTags.Player))
        {
            GetComponent<BoxCollider2D>().isTrigger = false;
            UpdateCurrentLevelAndSaveLevel();
        }
    }

    private void UpdateCurrentLevelAndSaveLevel()
    {
        _levelsDB.SetCurrentLevel(Type);
        _persistentPlayerData.PlayerData.OpenLevel(Type);
        _persistentPlayerData.SaveData();
    }
}
