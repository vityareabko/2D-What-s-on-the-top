using System;
using UnityEngine;
using Zenject;

public class TriggerPlayerTransionToOtherLevel : MonoBehaviour
{
    public SpawnPointType Type;
    
    private IPlayer _player;
    private PlayerConfig _playerConfig;
    
    [Inject] private void Construct(IPlayer player, PlayerConfig playerConfig)
    {
        _player = player;
        _playerConfig = playerConfig;
    }

    private void Awake()
    {
        if (_player.Transform.position.y > transform.position.y)
            GetComponent<BoxCollider2D>().isTrigger = false;
    }

    private void OnTriggerExit2D(Collider2D colider)
    {
        if (colider.CompareTag(ConstTags.Player))
        {
            GetComponent<BoxCollider2D>().isTrigger = false;
            _playerConfig.SetCurrentSpawnPoint(Type);
        }
    }
}
