using System;
using UnityEngine;

namespace User_Interface
{
    /// <summary>
    /// Makes the GameObject look at the camera, which is useful for UI elements that should always face the player, such as health bars or nameplates.
    /// </summary>
    public class LookAtCamera : MonoBehaviour
    {
        private Transform _cameraTransform;

        private void Start()
        {
            _cameraTransform = Camera.main?.transform;
            if (_cameraTransform == null)
            {
                Debug.LogError(
                    "LookAtCamera: No main camera found. Please ensure there is a camera in the scene tagged as 'MainCamera'.");
            }
        }

        private void LateUpdate()
        {
            if (_cameraTransform == null) return;

            transform.LookAt(transform.position + _cameraTransform.rotation * Vector3.forward,
                _cameraTransform.rotation * Vector3.up);
        }
    }
}
