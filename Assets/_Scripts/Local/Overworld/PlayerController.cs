using UnityEngine;
using TMPro;
using Enum;
using System.Collections;

namespace Overworld
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private TextMeshPro _playerNameText;
        [SerializeField] private Animator _animator;
        [SerializeField] private CharacterController _characterController;
        [SerializeField] private GameObject _chatBubble;
        [SerializeField] private TextMeshPro _chatBubbleText;
        [SerializeField] private float _chatBubbleDuration;
        [SerializeField] private float _moveSpeed;
        protected Vector3 _endPosition;
        private string _playerName;
        private PlayerAnimationState _animationState;
        protected int _leftRightDirection;
        protected int _forwardDirection;
        protected bool _grounded;
        private IEnumerator _bubbleCoroutine;
        private Transform _camera;
        private Vector3 _currentVelocity;

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

        protected virtual void MoveAdvanced()
        {
            transform.position = Vector3.SmoothDamp(transform.position, _endPosition, ref _currentVelocity, _moveSpeed * Time.fixedDeltaTime);
        }

        protected virtual void MoveBasic()
        {
            transform.position = _endPosition;
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
