using Obstacles;
using PersistentData;
using UnityEngine;
using Zenject;

namespace Levels
{
    public class LevelBehavior : MonoBehaviour
    {
        [SerializeField] private LevelType _type;
        [SerializeField] private Transform _spawnPoint;
        
        public LevelType Type => _type;
        public Transform SpawnPoint => _spawnPoint;
        
        private IPlayer _player;
        private LevelsDB _levelsDB;
        private IPersistentPlayerData _persistentPlayerData;
        
        private float bounceForceUp = 12f;
        private float bounceForceSide = 0.8f;
    
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
            else
                GetComponent<BoxCollider2D>().isTrigger = true;
            
        }

        private void OnTriggerExit2D(Collider2D colider)
        {
            if (colider.CompareTag(ConstTags.Player))
            {
                GetComponent<BoxCollider2D>().isTrigger = false;
                UpdateCurrentLevelAndSaveLevel();
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag(ConstTags.Player))
            {
                EventAggregator.Post(this, new SwitchGameStateToWinGameEvent());

                var playerTransform = collision.GetComponent<Transform>();
                playerTransform.rotation = Quaternion.identity;
            
                var rigidbody = collision.GetComponent<Rigidbody2D>();
                rigidbody.velocity = Vector2.zero;
            
                float sideDirection = (rigidbody.position.x >= 0) ? -1 : 1;
            
                Vector2 forceDirection = new Vector2(bounceForceSide * sideDirection, bounceForceUp);
                rigidbody.AddForce(forceDirection, ForceMode2D.Impulse);
            }
        }

        private void UpdateCurrentLevelAndSaveLevel()
        {
            _levelsDB.SetCurrentLevel(Type);
            _persistentPlayerData.PlayerData.OpenLevel(Type);
            _persistentPlayerData.SaveData();
        }
    
    }
}