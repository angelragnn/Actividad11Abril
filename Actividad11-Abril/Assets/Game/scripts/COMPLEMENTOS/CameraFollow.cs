using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Objetivo a seguir")]
    public Transform target;

    [Header("Suavizado")]
    public float smoothSpeed = 5f;

    [Header("Offset de la cámara")]
    public Vector3 offset = new Vector3(0f, 0f, -10f);

    void LateUpdate()
    {
        if (target == null) return;

        Vector3 desiredPosition = target.position + offset;

        Vector3 smoothedPosition = Vector3.Lerp(
            transform.position,
            desiredPosition,
            smoothSpeed * Time.deltaTime
        );

        transform.position = smoothedPosition;
    }
}