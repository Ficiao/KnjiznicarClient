using UnityEngine;

namespace Overworld
{
    public class CameraController : Singleton<CameraController>
    {
        [SerializeField] private int _zoomLevels;
        [SerializeField] private float _angularSpeed;
        [SerializeField] private float _maxAngleBuffer;
        [SerializeField] private float _maxCameraAngle;
        private Transform _endTarget;
        private Transform _startTarget;
        private int _currentZoom;
        private float _zoomPercentage;
        private Vector3 _worldPositionNoCollision;
        private LayerMask _layerMask;
        private UserController _userController;
        private bool _disableCameraMovement;
        private Vector3 _oldRotation;

        public bool DisableCameraMovement 
        { 
            get => _disableCameraMovement; 
            set
            {
                _disableCameraMovement = value;
                _userController.DisableCameraMovement = value;
            } 
        }

        private void Update()
        {
            if (_disableCameraMovement) return;

            if (Input.GetKey(KeyCode.Mouse0) || Input.GetKey(KeyCode.Mouse1))
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;

                float angle = _angularSpeed * Input.GetAxis("Mouse Y") * Time.deltaTime;
                _oldRotation = _endTarget.localEulerAngles;
                _endTarget.Rotate(angle, 0, 0);
                if (Mathf.Abs(_endTarget.localEulerAngles.x) > _maxCameraAngle && Mathf.Abs(_endTarget.localEulerAngles.x) < 360 - _maxCameraAngle)
                {
                    if (Mathf.Abs(_endTarget.localEulerAngles.x) < 360 / 2) _oldRotation.x = _maxCameraAngle;
                    else _oldRotation.x = 360 - _maxCameraAngle;
                    _endTarget.localEulerAngles = _oldRotation;
                }
            }
            else
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }

            if (Input.GetAxis("Mouse ScrollWheel") < 0f && _currentZoom < _zoomLevels)
            {
                _currentZoom++;
                _zoomPercentage = (float)_currentZoom / _zoomLevels;
            }
            else if (Input.GetAxis("Mouse ScrollWheel") > 0f && _currentZoom > 1)
            {
                _currentZoom--;
                _zoomPercentage = (float)_currentZoom / _zoomLevels;
            }

            transform.position = Vector3.Lerp(_endTarget.position, _startTarget.position, _zoomPercentage);
            transform.LookAt(_endTarget);
            _worldPositionNoCollision = transform.position;

            RaycastHit hit;
            if(Physics.SphereCast(_endTarget.position, 0.5f, _worldPositionNoCollision - _endTarget.position, out hit,
                Vector3.Distance(_worldPositionNoCollision, _endTarget.position), _layerMask))
            {
                transform.position = Vector3.Lerp(_endTarget.localPosition, hit.point, 0.975f); 
            }
        }

        public void Init(Transform endTarget, Transform startTarget, UserController userController)
        {
            _userController = userController;
            _endTarget = endTarget;
            _startTarget = startTarget;
            transform.localPosition = _startTarget.localPosition;
            _currentZoom = _zoomLevels;
            _zoomPercentage = 1f;
            transform.LookAt(_endTarget);
            _layerMask = LayerMask.GetMask("Ground");
        }
    }
}