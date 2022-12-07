using UnityEngine;
using TMPro;
using Enum;

namespace Overworld
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private float _lerpSpeed;
        [SerializeField] private TextMeshPro _playerNameText;
        [SerializeField] private Animator _animator;
        [SerializeField] private CharacterController _characterController;
        private Vector3 _endPosition;
        private string _playerName;
        private PlayerAnimationState _animationState;
        private int _leftRightDirection;
        private int _forwardDirection;
        private bool _grounded;

        protected void Start()
        {
            _animationState = PlayerAnimationState.Idle;
        }

        public string PlayerName
        {
            get => _playerName;
            set
            {
                _playerName = value;
                _playerNameText.text = value;
            }
        }
        protected void Update()
        {
            AnimationUpdate();
        }

        protected void FixedUpdate()
        {
            //transform.position = Vector3.MoveTowards(transform.position, _endPosition, Time.deltaTime * _lerpSpeed);
            _playerNameText.transform.LookAt(Camera.main.transform);
            _playerNameText.transform.Rotate(0, 180, 0);
        }

        public void NextPosition(Vector3 position, int leftRightDirection, int forwardDirection, bool grounded)
        {
            _leftRightDirection = leftRightDirection;
            _forwardDirection = forwardDirection;
            _grounded = grounded;
            transform.position = position;
            _endPosition = position;
        }

        public void NextPosition(Vector3 position, Vector3 rotation, int leftRightDirection, int forwardDirection, bool grounded)
        {
            _leftRightDirection = leftRightDirection;
            _forwardDirection = forwardDirection;
            _grounded = grounded;
            transform.position = position;
            transform.rotation = Quaternion.Euler(rotation);
            _endPosition = position;
            transform.rotation = Quaternion.Euler(rotation);
        }

        public void AnimationUpdate()
        {
            if (_grounded)
            {
                switch ((_leftRightDirection, _forwardDirection))
                {
                    case (1, 1):
                        if(_animationState != PlayerAnimationState.StrafingForwardRight)
                        {
                            _animationState = PlayerAnimationState.StrafingForwardRight;
                            _animator.SetTrigger("StrafeForwardRight");
                        }
                        break;
                    case (1, 0):
                        if (_animationState != PlayerAnimationState.StrafingRight)
                        {
                            _animationState = PlayerAnimationState.StrafingRight;
                            _animator.SetTrigger("StrafeRight");
                        }                        
                        break;
                    case (1, -1):
                        if (_animationState != PlayerAnimationState.StrafingBackRight)
                        {
                            _animationState = PlayerAnimationState.StrafingBackRight;
                            _animator.SetTrigger("StrafeBackRight");
                        }
                        break;
                    case (0, 1):
                        if (_animationState != PlayerAnimationState.RunningForward)
                        {
                            _animationState = PlayerAnimationState.RunningForward;
                            _animator.SetTrigger("RunForward");
                        }
                        break;
                    case (0, 0):
                        if (_animationState != PlayerAnimationState.Idle)
                        {
                            _animationState = PlayerAnimationState.Idle;
                            _animator.SetTrigger("Idle");
                        }
                        break;
                    case (0, -1):
                        if (_animationState != PlayerAnimationState.RunningBack)
                        {
                            _animationState = PlayerAnimationState.RunningBack;
                            _animator.SetTrigger("RunBack");
                        }
                        break;
                    case (-1, 1):
                        if (_animationState != PlayerAnimationState.StrafingForwardLeft)
                        {
                            _animationState = PlayerAnimationState.StrafingForwardLeft;
                            _animator.SetTrigger("StrafeForwardLeft");
                        }
                        break;
                    case (-1, 0):
                        if (_animationState != PlayerAnimationState.StrafingLeft)
                        {
                            _animationState = PlayerAnimationState.StrafingLeft;
                            _animator.SetTrigger("StrafeLeft");
                        }
                        break;
                    case (-1, -1):
                        if (_animationState != PlayerAnimationState.StrafingBackLeft)
                        {
                            _animationState = PlayerAnimationState.StrafingBackLeft;
                            _animator.SetTrigger("StrafeBackLeft");
                        }
                        break;
                    default:
                        break;
                }
            }
            else
            {
                if (_animationState != PlayerAnimationState.Jumping)
                {
                    _animator.SetBool("Jumping", true);
                    _animationState = PlayerAnimationState.Jumping;
                }
            }
        }
    }
}
