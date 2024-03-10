using UnityEngine;

public class PlayerBounceToMainMenuOnPlatform : MonoBehaviour
{
   
    private float bounceForceUp = 12f;
    private float bounceForceSide = 0.8f;

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
}
