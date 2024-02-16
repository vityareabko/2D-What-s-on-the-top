using UnityEngine;

namespace Obstacles
{
    public class FallObstacle : MonoBehaviour
    {
        private float _speed;
        public void Initialize(float speed) => _speed = speed;

        private void Update() => FallDown();

        public void FallDown() => transform.Translate(Vector3.down * _speed * Time.deltaTime);
    }
}