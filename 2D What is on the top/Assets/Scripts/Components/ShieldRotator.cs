using UnityEngine;

public class ShieldRotator : MonoBehaviour
{
    private float _rotationSpeed = 700f;

    private void Update()
    {
        transform.Rotate(Vector3.forward, _rotationSpeed * Time.deltaTime, Space.World);
    }
}
