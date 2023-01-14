using KnjiznicarDataModel.Message;
using Network;
using System.Collections;
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
        private Npc _interactNpc;

        public bool DisableCameraMovement;
        public bool Enabled;

        private void Start()
        {
            base.Start();
            Enabled = true;
            CameraController.Instance.transform.parent = transform;
            CameraController.Instance.Init(_cameraEndTarget, _cameraStartTarget, this);
            _rotation = new Vector3(0, 0, 0);

            StartCoroutine(HeartBeatSender());
        }

        private void Update()
        {
            base.Update();
            if (!Enabled)
            {
                _sendForwardDirection = 0;
                _sendLeftRightDirection = 0;
                _jump = false;
                return;
            }

            if ((Input.GetKey(KeyCode.Mouse0) || Input.GetKey(KeyCode.Mouse1)) && DisableCameraMovement == false)
            {
                _rotation.y = transform.localEulerAngles.y + Input.GetAxis("Mouse X") * _angularSpeed * Time.deltaTime;
                transform.localEulerAngles = _rotation;
            }
            _bothMouseButtons = Input.GetKey(KeyCode.Mouse0) && Input.GetKey(KeyCode.Mouse1);

            _sendLeftRightDirection = (Input.GetKey(KeyCode.A) ? -1 : 0) + (Input.GetKey(KeyCode.D) ? 1 : 0);
            _sendForwardDirection = (Input.GetKey(KeyCode.S) ? -1 : 0) + (Input.GetKey(KeyCode.W) || _bothMouseButtons ? 1 : 0);
            _jump = Input.GetKey(KeyCode.Space);

            if(_interactNpc != null && Input.GetKeyDown(KeyCode.F))
            {
                _interactNpc.Interact();
            }
        }

        private void FixedUpdate()
        {
            base.FixedUpdate();
        }

        protected override void MoveAdvanced()
        {
            MoveBasic();
        }

        protected override void MoveBasic()
        {
            Vector3 rotation = transform.rotation.eulerAngles;
            PlayerInputMessage message = new PlayerInputMessage()
            {
                LeftRightDirection = _sendLeftRightDirection,
                ForwardDirection = _sendForwardDirection,
                Jump = _jump,
                Rotation = new float[3] { rotation.x, rotation.y, rotation.z },
            };
            ClientSend.SendUDPData(message, Client.OverworldServer);
        }

        private IEnumerator HeartBeatSender()
        {
            HeartBeatMessage message = new HeartBeatMessage();

            while (true)
            {
                yield return new WaitForSecondsRealtime(120);
                ClientSend.SendTCPData(message, Client.OverworldServer);
            }
        }

        public void NextPosition(Vector3 position, int leftRightDirection, int forwardDirection, bool grounded)
        {
            _grounded = grounded;
            _leftRightDirection = leftRightDirection;
            _forwardDirection = forwardDirection;
            transform.position = position;
            _endPosition = position;
        }

        public void SetNpcToInteract(Npc npc)
        {
            _interactNpc = npc;
            UIManager.Instance.ShowInteractTooltip();
        }

        public void RemoveNpcInteract()
        {
            _interactNpc = null;
            UIManager.Instance.HideInteractTooltip();
        }
    }
}
