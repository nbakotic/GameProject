using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Camera Parameters")]
    public Transform _target;
    public float _smoothSpeed = 0.125f;
    public Vector3 _offset;

    void FixedUpdate()
    {
        Vector3 desiredPosition = _target.position + _offset;
        Vector3 smoothedPositon = Vector3.Lerp(transform.position, desiredPosition, _smoothSpeed);
        transform.position = smoothedPositon;
    }
}
