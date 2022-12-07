using KnjiznicarDataModel.Message;
using Network;
using UnityEngine;

namespace Overworld
{
    public class UserController : PlayerController
    {
        [SerializeField] private Transform _cameraEndTarget;
        [SerializeField] private Transform _cameraStartTarget;
        [SerializeField] private float _angularSpeed;
        private int _sendLeftRightDirection;
        private int _sendForwardDirection;
        private bool _jump;
        private bool _bothMouseButtons;
        private Vector3 _rotation;

        private void Start()
        {
            base.Start();
            CameraController.Instance.Init(_cameraEndTarget, _cameraStartTarget);
            _rotation = new Vector3(0, 0, 0);
        }

        private void Update()
        {
            base.Update();

            //if (Input.GetKey(KeyCode.Mouse0) || Input.GetKey(KeyCode.Mouse1))
            //{
            //    _rotation.y = transform.localEulerAngles.y - Input.GetAxis("Mouse X") * _angularSpeed * Time.deltaTime;
            //    transform.localEulerAngles = _rotation;
            //}
            _bothMouseButtons = Input.GetKey(KeyCode.Mouse0) && Input.GetKey(KeyCode.Mouse1);

            _sendLeftRightDirection = (Input.GetKey(KeyCode.A) ? -1 : 0) + (Input.GetKey(KeyCode.D) ? 1 : 0);
            _sendForwardDirection = (Input.GetKey(KeyCode.S) ? -1 : 0) + (Input.GetKey(KeyCode.W) || _bothMouseButtons ? 1 : 0);
            _jump = Input.GetKey(KeyCode.Space);
        }

        private void FixedUpdate()
        {
            base.FixedUpdate();
            Vector3 rotation = transform.rotation.eulerAngles;
            PlayerInputMessage message = new PlayerInputMessage()
            {
                leftRightDirection = _sendLeftRightDirection,
                forwardDirection = _sendForwardDirection,
                jump = _jump,
                rotation = new float[3] { rotation.x, rotation.y, rotation.z },
            };
            ClientSend.SendUDPData(message, Client.OverworldServer);
        }
    }
}
