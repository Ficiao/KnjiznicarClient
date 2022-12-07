using UnityEngine;

namespace Overworld
{
    public class CameraController : Singleton<CameraController>
    {
        [SerializeField] private int _zoomLevels;
        [SerializeField] private float _angularSpeed;
        private Transform _endTarget;
        private Transform _startTarget;
        private int _currentZoom;
        private Vector3 _temp;
        private float _zoomPercentage;
        private bool _change;
        private float _startXPosition;

        private void Update()
        {
            _change = false;
            if (Input.GetKey(KeyCode.Mouse0) || Input.GetKey(KeyCode.Mouse1))
            {
                float angle = _angularSpeed * Input.GetAxis("Mouse Y") * Time.deltaTime;
                _startTarget.RotateAround(_endTarget.position, Vector3.right, angle);
                _temp.x = _startXPosition;
                _temp.y = _startTarget.position.y;
                _temp.z = _startTarget.position.z;
                _startTarget.position = _temp;
                _change = true;
            }

            if (Input.GetAxis("Mouse ScrollWheel") < 0f && _currentZoom < _zoomLevels)
            {
                _currentZoom++;
                _change = true;
            }
            else if (Input.GetAxis("Mouse ScrollWheel") > 0f && _currentZoom > 0)
            {
                _currentZoom--;
                _change = true;
            }

            if (_change)
            {
                transform.localPosition = Vector3.Lerp(_endTarget.localPosition, _startTarget.localPosition, _zoomPercentage);
                transform.LookAt(_endTarget);
            }
        }

        public void Init(Transform endTarget, Transform startTarget)
        {
            _endTarget = endTarget;
            _startTarget = startTarget;
            transform.localPosition = _startTarget.localPosition;
            _currentZoom = _zoomLevels;
            _zoomPercentage = 1f;
            _startXPosition = _startTarget.position.x;
            transform.LookAt(_endTarget);
        }
    }
}