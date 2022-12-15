using UnityEngine;

namespace Overworld
{
    public class CameraController : Singleton<CameraController>
    {
        [SerializeField] private int _zoomLevels;
        [SerializeField] private float _angularSpeed;
        [SerializeField] private float _maxAngleBuffer;
        private Transform _endTarget;
        private Transform _startTarget;
        private int _currentZoom;
        private Vector3 _temp;
        private float _zoomPercentage;
        private float _startXPosition;
        private Vector3 _worldPositionNoCollision;
        private LayerMask _layerMask;
        private Vector3 _preRotation;
        private UserController _userController;
        private bool _disableCameraMovement;

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

                float angle = _angularSpeed * Input.GetAxis("Mouse Y") * Time.deltaTime * -1;
                _preRotation = _startTarget.localPosition;
                _startTarget.RotateAround(_endTarget.position, Vector3.right, angle);
                _startTarget.LookAt(_endTarget);
                if(_startTarget.localPosition.z > _endTarget.localPosition.z)
                {
                    _startTarget.localPosition = _preRotation;
                }
                else
                {
                    _temp.x = _startXPosition;
                    _temp.y = _startTarget.localPosition.y;
                    _temp.z = _startTarget.localPosition.z;
                    _startTarget.localPosition = _temp;
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

            transform.localPosition = Vector3.Lerp(_endTarget.localPosition, _startTarget.localPosition, _zoomPercentage);
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
            _startXPosition = _startTarget.localPosition.x;
            transform.LookAt(_endTarget);
            _layerMask = LayerMask.GetMask("Ground");
            _startTarget.LookAt(_endTarget);
        }
    }
}