using UnityEngine;


public class FollowCamera : MonoBehaviour
{
    [SerializeField] private Transform _target; // Цель, за которой следует камера.
    [SerializeField] private Vector3 _offset = new Vector3(0, 5, -10); // Смещение относительно цели.
    [SerializeField] private float _followSpeed = 10f; // Скорость, с которой камера следует за целью.
    [SerializeField] private float _smoothness = 0.125f; // Плавность следования камеры (значение между 0 и 1).

    private void LateUpdate()
    {
        if (_target == null)
            return;

        // Вычисление позиции, куда должна переместиться камера
        Vector3 desiredPosition = _target.position + _offset;

        // Плавное перемещение камеры к желаемой позиции с использованием Lerp
        var transformPosition = transform.position;
        Vector3 smoothedPosition = Vector3.Lerp(transformPosition, desiredPosition, _smoothness * _followSpeed * Time.deltaTime);

        // Установка новой позиции камеры
        transformPosition.x = smoothedPosition.x;
        transformPosition.y = _target.position.y + _offset.y;
        transform.position = transformPosition;
    }
}
