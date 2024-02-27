
using Triggers;
using UnityEngine;

public class PlayerBounceToMainMenuOnPlatform : MonoBehaviour
{
    public float jumpForce = 10f;
    public float rotationDuration = 1f;
    
    private Player _player;
    private LastJumpToCenter _lastJump;

    private void Update()
    {
        if (_lastJump != null)
            _lastJump.Tick();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(ConstTags.Player))
        {
            Rigidbody2D rb = collision.GetComponent<Rigidbody2D>();
            _player = collision.GetComponent<Player>();
            _player.enabled = false;

            _lastJump = new LastJumpToCenter(_player, jumpForce, rotationDuration);
            _lastJump.StartLastJump();
            
            // EventAggregator ... SwitchGameStateToWinState ... # todo - подумать как доделать все это дела ( проблеба в том что мне нужно заблокировать игрока пока он ему ну будет показываться менюшка)
        }
    }
}

