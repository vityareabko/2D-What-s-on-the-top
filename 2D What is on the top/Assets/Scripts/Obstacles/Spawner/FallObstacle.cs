using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;


namespace Obstacles
{
    public class FallObstacle : MonoBehaviour
    {
        private const float _lifeDelay = 3f;
        private const float _minRotationSpeed = 15f;
        private const float _maxRotationSpeed = 70f;
        
        private FallingObstaclesType _fallingObstaclesType;
        private float _speed;
        private float _directionRotate;

        public void Initialize(float speed, FallingObstaclesType fallingObstaclesType)
        {
            _speed = speed;
            _fallingObstaclesType = fallingObstaclesType;
            _directionRotate = Random.value < 0.5 ? -1 : 1;
            StartCoroutine(LifeDelayCoroutine());
        }

        private void Update()
        {
            AddDynamicRotation();
        }

        // использую Fixed Update потому что в камере я использую ограничение кадров для камеры (чтобы она не тряслась) и теперь если я использую Update то не очень хорошо ведет себя постоянно движущиеся объекты 
        private void FixedUpdate() => FallDown();

        private void FallDown() => transform.Translate(Vector3.down * _speed * Time.fixedDeltaTime, Space.World);
        
        private void AddDynamicRotation()
        {
            float rotationSpeed = Random.Range(_minRotationSpeed,_maxRotationSpeed);
            transform.Rotate(0, 0, rotationSpeed * Time.deltaTime * _directionRotate, Space.World);
        }
        
        private IEnumerator LifeDelayCoroutine()
        {
            yield return new WaitForSeconds(_lifeDelay);
            Destroy(gameObject);
        }

        private void OnCollisionEnter2D(Collision2D collision) // # TODO - Прикольно было бы сделать иногда базу данных которая будет хранить фразочки и при сталкновения с игроком будет всплывать (если рандом будет randomNumber > 0.5 чтобы не очень часто показывать) 
        {
            if (collision.gameObject.CompareTag(ConstTags.Player))
            {
                // TODO - приоритет 3
                // # Todo - делаем всплывающие окно для фразочок или можно сделать через игрока когда в него попали то мы делаем инвоке и те ето подписаны на это реагирует а именно класс который отвечает за эти всплывающие фразочки 
                // делаем доступ к чтению типа который указан выше и это если я решу делать кто это говорить а если нет то тип вообще можно убирать так как он не нуден пока что
                
                Debug.Log($"Tы че ахуел, смотри куда идешь ? P.S {_fallingObstaclesType}");
            }
        }
    }
}