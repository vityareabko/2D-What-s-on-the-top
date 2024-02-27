using System.Collections;
using Extensions;
using Helper;
using UnityEngine;

namespace Triggers
{
    public class LastJumpToCenter
    {
        private float _jumpForce;
        private float _rotationDuration;

        private Player _player;
        
        public LastJumpToCenter(Player player, float jumpForce, float rotationDuration)
        {
            _jumpForce = jumpForce;
            _rotationDuration = rotationDuration;
            _player = player;
        }

        public void StartLastJump()
        {
            if (_player.TryGetComponent(out Rigidbody2D rigidbody))
            {
                _player.enabled = false;
                rigidbody.AddForce(Vector2.up * _jumpForce, ForceMode2D.Impulse);
                
                _player.StartCoroutine(CenterPlayer(rigidbody));
                _player.StartCoroutine(RotatePlayerToUp(_player.GetComponent<Transform>()));
            }
        }

        public void Tick()
        {
            CheckGround();
        }

        private IEnumerator CenterPlayer(Rigidbody2D rb)
        {
            float initialX = rb.position.x;
            float g = Mathf.Abs(Physics2D.gravity.y * rb.gravityScale);
            float v = _jumpForce / rb.mass; // Предполагаем, что jumpForce пропорционален начальной скорости
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
        }

        private IEnumerator RotatePlayerToUp(Transform playerTransform)
        {
            float totalTime = 0; 
            float startRotationZ = playerTransform.eulerAngles.z; 

            while (totalTime < _rotationDuration)
            {
                totalTime += Time.deltaTime;
                float fraction = totalTime / _rotationDuration;
            
                float newZ = Mathf.LerpAngle(startRotationZ, 0, fraction);
                playerTransform.eulerAngles = new Vector3(playerTransform.eulerAngles.x, playerTransform.eulerAngles.y, newZ);

                yield return null;
            }
        
            playerTransform.eulerAngles = new Vector3(playerTransform.eulerAngles.x, playerTransform.eulerAngles.y, 0);
        }

        private void CheckGround() 
        {
            var checkPatformMainMenu = Physics2D.OverlapCircle(_player.transform.position, 1f, ConstLayer.PlatformMainMenu.ToLayerMask());
            if (checkPatformMainMenu)
                _player.enabled = true;
        }
    }
}