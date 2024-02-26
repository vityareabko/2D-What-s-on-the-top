using System.Collections;
using UnityEngine;

public class PlayerBounceToMainMenuOnPlatform : MonoBehaviour
{
    public float jumpForce = 10f;
    public float rotationDuration = 1f;
    
    private Player _player;
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(ConstTags.Player))
        {
            Rigidbody2D rb = collision.GetComponent<Rigidbody2D>();
            _player = collision.GetComponent<Player>();
            _player.enabled = false;
            
            collision.GetComponent<Animator>().SetFloat("Speed", 0);
            
            if (rb != null)
            {
                rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
                
                StartCoroutine(CenterPlayer(rb));
                StartCoroutine(RotatePlayerToUp(collision.transform));
            }
        }
    }

    private IEnumerator CenterPlayer(Rigidbody2D rb)
    {
        float initialX = rb.position.x;
        float g = Mathf.Abs(Physics2D.gravity.y * rb.gravityScale);
        float v = jumpForce / rb.mass; // Предполагаем, что jumpForce пропорционален начальной скорости
        float timeToApex = v / g;

        float startTime = Time.time;
        float journeyLength = Mathf.Abs(initialX - 0);
        float centeringSpeedDynamic = journeyLength / timeToApex;

        while (Mathf.Abs(rb.position.x) > 0.01f)
        {
            float distCovered = (Time.time - startTime) * centeringSpeedDynamic;
            float fractionOfJourney = distCovered / journeyLength;

            float newX = Mathf.Lerp(initialX, 0, fractionOfJourney);
            rb.position = new Vector2(newX, rb.position.y);
            
            yield return null;
        }
        
        
        _player.enabled = true;
    }

    private IEnumerator RotatePlayerToUp(Transform playerTransform)
    {
        float totalTime = 0; 
        float startRotationZ = playerTransform.eulerAngles.z; 

        while (totalTime < rotationDuration)
        {
            totalTime += Time.deltaTime;
            float fraction = totalTime / rotationDuration;
            
            float newZ = Mathf.LerpAngle(startRotationZ, 0, fraction);
            playerTransform.eulerAngles = new Vector3(playerTransform.eulerAngles.x, playerTransform.eulerAngles.y, newZ);

            yield return null;
        }
        
        playerTransform.eulerAngles = new Vector3(playerTransform.eulerAngles.x, playerTransform.eulerAngles.y, 0);
    }
}

