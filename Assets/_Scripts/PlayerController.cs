using Assets._Scripts.Network;
using Assets._Scripts.Network.Message;
using UnityEngine;

namespace Assets._Scripts
{
    public class PlayerController : MonoBehaviour
    {
        private int _leftRightDirection;
        private int _forwardDirection;
        private bool _jump;

        private void Update()
        {
            _leftRightDirection = (Input.GetKey(KeyCode.A) ? -1 : 0) + (Input.GetKey(KeyCode.D) ? 1 : 0);
            _forwardDirection = (Input.GetKey(KeyCode.S) ? -1 : 0) + (Input.GetKey(KeyCode.W) ? 1 : 0);
            _jump = Input.GetKey(KeyCode.Space);
        }

        private void FixedUpdate()
        {
            PlayerInputMessage message = new PlayerInputMessage()
            { 
                leftRightDirection = _leftRightDirection,
                forwardDirection = _forwardDirection,
                jump = _jump
            };
            //ClientSend.SendTCPData(message, Client.Instance.OverworldServer);
        }
    }
}
