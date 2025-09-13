using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform cameraTarget;
    [SerializeField] private InputAction zoomInputAction;
    [SerializeField] private float offsetX = 0f;
    [SerializeField] private float offsetY = 5f;
    [SerializeField] private float offsetZ = -10f;
    [SerializeField] private float maxCameraDistance = 15f;
    [SerializeField] private float minCameraDistance = 3f;
    [SerializeField] private float cameraFollowSpeed = 5f;
    [SerializeField] private float cameraZoomSpeed = 2f;
    
    private Camera _camera;
    private float _currentDistance = 8f;

    private void OnEnable()
    {
        zoomInputAction.Enable();
    }

    private void OnDisable()
    {
        zoomInputAction.Disable();
    }

    private void Start()
    {
        _camera = Camera.main;
        // Initialize the current distance based on the initial offset
        _currentDistance = Mathf.Sqrt(offsetX * offsetX + offsetY * offsetY + offsetZ * offsetZ);
        _currentDistance = Mathf.Clamp(_currentDistance, minCameraDistance, maxCameraDistance);
        _currentDistance = maxCameraDistance;

    }

    private void Update()
    {
        if (cameraTarget == null) return;

        // Handle zoom input
        float zoomInput = zoomInputAction.ReadValue<float>();
        _currentDistance += zoomInput * cameraZoomSpeed * Time.deltaTime;
        _currentDistance = Mathf.Clamp(_currentDistance, minCameraDistance, maxCameraDistance);

        // Calculate the desired position with the offset, maintaining the camera's own rotation
        Vector3 offset = new Vector3(offsetX, offsetY, offsetZ).normalized * _currentDistance;
        Vector3 desiredPosition = cameraTarget.position + offset;

        // Smoothly move the camera to the desired position
        _camera.transform.position = Vector3.Lerp(_camera.transform.position, 
            desiredPosition, cameraFollowSpeed * Time.deltaTime);
    }
}
