using UnityEngine;
using System.Collections;

[AddComponentMenu("Camera-Control/Mouse Orbit with zoom")]
public class OrbitCamera : MonoBehaviour
{
    public Transform target;

    public float x = 0.2f;
    public float y = 0.3f;
    public float z = 0.9f;

    public float xSpeed = 2.5f;
    public float ySpeed = 5f;
    public float zSpeed = 5f;

    [HideInInspector]
    public Vector3 velocity;
    public float xAngle = 1f;
    public float yMin = 0;
    public float yMax = 100;
    public float zMin = 6f;
    public float zMax = 20;
    public float dampingTime = 1f;
    public float cameraWaitDelay = 10f;
    public float movingSpeedX = 0.3f;
    public float movingSpeedY = 0.3f;
    
    private float _waitAfterUserInput = 0;
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            _waitAfterUserInput = cameraWaitDelay;
            x += Input.GetAxis("Mouse X") * xSpeed * Time.deltaTime;
            y -= Input.GetAxis("Mouse Y") * ySpeed * Time.deltaTime;
        }
        z -= Input.GetAxis("Mouse ScrollWheel") * zSpeed * 0.02f;

        _waitAfterUserInput -= Time.deltaTime;
        if (_waitAfterUserInput < 0)
        {
            x = xAngle * Mathf.Sin(movingSpeedX * Time.time);
            y = Mathf.Abs(Mathf.Cos(movingSpeedY * Time.time));
        }
        x = Mathf.Clamp(x, -xAngle, xAngle);
        x = x >= 1 ? -1 : (x <= -1 ? 1 : x);
        y = Mathf.Clamp01(y);
        z = Mathf.Clamp01(z);
    }

    void LateUpdate()
    {
        if (target)
        {
            Quaternion rotation = Quaternion.Euler(y * (yMax - yMin) + yMin, x * 360, 0);
            transform.position = Vector3.SmoothDamp(transform.position, rotation * new Vector3(0, 0, -(z * (zMax - zMin) + zMin)) + target.position, ref velocity, dampingTime);

            Vector3 relativePos = target.position - transform.position;
            transform.rotation = Quaternion.LookRotation(relativePos);
        }
    }
}