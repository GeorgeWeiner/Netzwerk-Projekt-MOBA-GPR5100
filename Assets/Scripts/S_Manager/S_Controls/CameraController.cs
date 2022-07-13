using System;
using S_Unit;
using UnityEngine;
using Screen = UnityEngine.Device.Screen;

namespace S_Manager.S_Controls
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private GameObject camera;
        [SerializeField] private float cameraSpeed;
        [SerializeField] private float maxMagnitude;

        private int _screenWidth, _screenHeight;
        private Vector2 _mousePositionLastFrame;

        private UnitSelectionHandler _unitSelectionHandler;

        private void Start()
        {
            _screenWidth = Screen.width;
            _screenHeight = Screen.height;

            _mousePositionLastFrame = Vector2.zero;

            _unitSelectionHandler = FindObjectOfType<UnitSelectionHandler>();

            InputManager.OnResetCamera += ResetCamera;
            
            if (camera != null) return;
            if (Camera.main != null) camera = Camera.main.gameObject;
        }

        private void Update()
        {
            EdgePanning();
            MiddleMouseMovement();
        }

        private Vector3 GetMouseDirection()
        {
            var direction = InputManager.instance.GetMousePos() - _mousePositionLastFrame;
            _mousePositionLastFrame = InputManager.instance.GetMousePos();
            
            return new Vector3(direction.x, 0f, direction.y);
        }

        private void EdgePanning()
        {
            
        }
        
        private void ResetCamera()
        {
            var position = 
            
            
        }
        
        private void MiddleMouseMovement()
        {
            if (InputManager.instance.HoldingMiddleMouseButton)
            {
                var difference = Vector3.ClampMagnitude(GetMouseDirection() * cameraSpeed, maxMagnitude);
                camera.transform.position += 
                    difference * (Mathf.Pow(cameraSpeed, Mathf.Clamp(difference.magnitude, 0, maxMagnitude))
                                  * Time.deltaTime);
            }
        }
    }
}