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
        [SerializeField] private Vector2 targetOffset;
        [SerializeField] private Vector2 camPosMax;
        [SerializeField] private Vector2 camPosMin;

        [SerializeField] private bool enableEdgePanning = true;
        [SerializeField] private bool enableMiddleMouse = true;

        private int _screenWidth, _screenHeight;
        private Vector2 _mousePositionLastFrame;
        private float _camPosYStart;

        private UnitSelectionHandler _unitSelectionHandler;

        private void Start()
        {
            _screenWidth = Screen.width;
            _screenHeight = Screen.height;

            _mousePositionLastFrame = Vector2.zero;

            _unitSelectionHandler = FindObjectOfType<UnitSelectionHandler>();

            _camPosYStart = transform.position.y;

            InputManager.OnResetCamera += ResetCamera;
            
            if (camera != null) return;
            if (Camera.main != null) camera = Camera.main.gameObject;
        }

        private void Update()
        {
            EdgePanning();
            MiddleMouseMovement();
            ClampCameraMovement();
        }

        private void EdgePanning()
        {
            if (!enableEdgePanning) return;
            
            if (InputManager.instance.GetMousePos().x >= _screenWidth - 1)
                transform.position += Vector3.left * (cameraSpeed * 30 * Time.deltaTime);
            
            if (InputManager.instance.GetMousePos().y >= _screenHeight - 1)
                transform.position += Vector3.back * (cameraSpeed * 30 * Time.deltaTime);

            if (InputManager.instance.GetMousePos().x <= 1)
                transform.position += Vector3.right * (cameraSpeed * 30 * Time.deltaTime);
            
            if (InputManager.instance.GetMousePos().y <= 1)
                transform.position += Vector3.forward * (cameraSpeed * 30 * Time.deltaTime);
        }

        private void MiddleMouseMovement()
        {
            if (!enableMiddleMouse) return;
            
            if (InputManager.instance.HoldingMiddleMouseButton)
            {
                var difference = Vector3.ClampMagnitude(GetMouseDirection() * cameraSpeed, maxMagnitude);
                
                camera.transform.position += 
                    difference * (Mathf.Pow(cameraSpeed, Mathf.Clamp(difference.magnitude, 0, maxMagnitude)) 
                                  * Time.deltaTime);
            }
        }
        
        private Vector3 GetMouseDirection()
        {
            var direction = InputManager.instance.GetMousePos() - _mousePositionLastFrame;
            _mousePositionLastFrame = InputManager.instance.GetMousePos();
            
            return new Vector3(direction.x, 0f, direction.y);
        }
        
        //b is the offset to the target in either x or z axis.
        //b = sqrt (c^2 - a^2)
        private void ResetCamera()
        {
            //Physics.Raycast(camPosition, Vector3.down, out var hit, float.MaxValue, map);
            //
            ////Use inverse pythagorean theorem.
            //var cx = camPosition.x - target.x;
            //var cz = camPosition.z - target.z;
            //var yDiff = camPosition.y - hit.point.y;
            
            if (_unitSelectionHandler.selectedUnits.Count == 0) return;
            
            var target = _unitSelectionHandler.selectedUnits[0].transform.position;

            var cameraTargetRaw = target + new Vector3(targetOffset.x, 0f, targetOffset.y);
            transform.position = new Vector3(cameraTargetRaw.x, _camPosYStart, cameraTargetRaw.z);
        }

        private void ResetHoldCamera()
        {
            
        }
        
        private void ClampCameraMovement()
        {
            var max = new Vector3(camPosMax.x, _camPosYStart, camPosMax.y);
            var min = new Vector3(camPosMin.x, _camPosYStart, camPosMin.y);

            transform.position = 
                new Vector3(Mathf.Clamp(transform.position.x, min.x, max.x),
                _camPosYStart, Mathf.Clamp(transform.position.z, min.z, max.z));
        }
    }
}