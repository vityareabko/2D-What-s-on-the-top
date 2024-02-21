    using System;
    using System.Collections;
    using UnityEngine;
    using Random = UnityEngine.Random;


    namespace Obstacles
    {
        public abstract class FallObjectBase : MonoBehaviour
        {
            private const float _lifeDelay = 3f;
            private const float _minRotationSpeed = 15f;
            private const float _maxRotationSpeed = 70f;
            
            protected float _speed;
            private float _directionRotate;
            
            public void Initialize(float speed)
            {
                _speed = speed;
                
                _directionRotate = Random.value < 0.5 ? -1 : 1;
                StartCoroutine(LifeDelayCoroutine());
            }
          
            private void Update()
            {
                AddDynamicRotation();
                FallDown();
            }   

            private void FallDown()
            {
                transform.Translate(Vector3.down * _speed * Time.deltaTime, Space.World);
            }

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
        }
    }