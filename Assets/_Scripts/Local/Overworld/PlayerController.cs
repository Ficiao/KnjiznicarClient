using UnityEngine;
using TMPro;
using Enum;
using System.Collections;

namespace Overworld
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private TextMeshPro _playerNameText;
        [SerializeField] protected Animator _animator;
        [SerializeField] protected CharacterController _characterController;
        [SerializeField] private GameObject _chatBubble;
        [SerializeField] private TextMeshPro _chatBubbleText;
        [SerializeField] private float _chatBubbleDuration;
        [SerializeField] protected float _moveSpeed;
        [SerializeField] private float _snapThreshold;
        protected Vector3 _endPosition;
        private string _playerName;
        protected PlayerAnimationState _animationState;
        protected int _leftRightDirection;
        protected int _forwardDirection;
        protected bool _grounded;
        private IEnumerator _bubbleCoroutine;
        private Transform _camera;
        private Vector3 _currentVelocity;
        protected Vector3 _moveDirection;

        public bool AdvancedMovementEnabled;

        protected void Start()
        {
            _animationState = PlayerAnimationState.Idle;
            _camera = Camera.main.transform;
            AdvancedMovementEnabled = true;
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
            _playerNameText.transform.LookAt(_camera);
            _playerNameText.transform.Rotate(0, 180, 0);
            PlayerAnimationController.AnimationUpdate(_grounded, _leftRightDirection, _forwardDirection, ref _animationState, _animator);
        }

        protected void FixedUpdate()
        {
            if (AdvancedMovementEnabled) MoveAdvanced();
            else MoveBasic();
        }

        private void MoveAdvanced()
        {
            //_moveDirection = _endPosition - transform.position;
            //if (_moveDirection.magnitude > _snapThreshold) _characterController.Move(_moveDirection.normalized * _moveSpeed * Time.fixedDeltaTime);
            //else MoveBasic();
            transform.position = Vector3.SmoothDamp(transform.position, _endPosition, ref _currentVelocity, _moveSpeed * Time.fixedDeltaTime);
        }

        private void MoveBasic()
        {
            _characterController.enabled = false;
            transform.position = _endPosition;
            _characterController.enabled = true;
        }

        public void NextPosition(Vector3 position, Vector3 rotation, int leftRightDirection, int forwardDirection, bool grounded)
        {
            _grounded = grounded;
            _leftRightDirection = leftRightDirection;
            _forwardDirection = forwardDirection;
            transform.rotation = Quaternion.Euler(rotation);
            _endPosition = position;
            transform.rotation = Quaternion.Euler(rotation);
        }       

        public void ShowText(string text)
        {
            _chatBubble.gameObject.SetActive(true);
            _chatBubbleText.text = text;
            if(_bubbleCoroutine != null)
            {
                StopCoroutine(_bubbleCoroutine);
            }
            _bubbleCoroutine = TextTimer();
            StartCoroutine(_bubbleCoroutine);
        }

        private IEnumerator TextTimer()
        {
            yield return new WaitForSeconds(_chatBubbleDuration);
            _chatBubble.gameObject.SetActive(false);
        }
    }
}
