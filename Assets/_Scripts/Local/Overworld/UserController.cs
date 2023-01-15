﻿using KnjiznicarDataModel;
using KnjiznicarDataModel.Message;
using Network;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Overworld
{
    public class UserController : PlayerController
    {
        private const float _SQUARE_OF_TWO = 1.41421356237f;

        [SerializeField] private Transform _cameraEndTarget;
        [SerializeField] private Transform _cameraStartTarget;
        [SerializeField] private float _angularSpeed;
        [SerializeField] private float _serverTickRate;
        [SerializeField] private int _bufferSize;
        [SerializeField] private float _reconciliationErrorMargin;
        [SerializeField] private float _gravity;
        [SerializeField] private float _jumpSpeed;
        private bool _jump;
        private bool _bothMouseButtons;
        private Vector3 _rotation;
        private Npc _interactNpc;
        private float _timer;
        private int _currentTick;
        private float _minTimeBetweenTicks;
        private PlayerState[] _stateBuffer;
        private PlayerInput[] _inputBuffer;
        private PlayerState _latestServerState;
        private PlayerState _lastProcessedState;
        private Vector2 _latestVector;
        private Vector2 _bufferVector;
        private PlayerInputMessage _message;
        private float _yVelocity;

        public bool DisableCameraMovement;
        public bool Enabled;

        private void Start()
        {
            base.Start();
            Enabled = true;
            CameraController.Instance.transform.parent = transform;
            CameraController.Instance.Init(_cameraEndTarget, _cameraStartTarget, this);
            _rotation = new Vector3(0, 0, 0);
            _moveDirection = new Vector3();
            _minTimeBetweenTicks = 1f / _serverTickRate;
            _stateBuffer = new PlayerState[_bufferSize];
            _inputBuffer = new PlayerInput[_bufferSize];
            _latestVector = new Vector3();
            _bufferVector = new Vector3();
            _message = new PlayerInputMessage();
            _latestServerState = new PlayerState();
            _lastProcessedState = _latestServerState;

            _gravity *= Time.fixedDeltaTime * Time.fixedDeltaTime;
            _moveSpeed *= _minTimeBetweenTicks;
            _jumpSpeed *= _minTimeBetweenTicks;

            StartCoroutine(HeartBeatSender());
        }

        private void Update()
        {
            if (!AdvancedMovementEnabled)
            {
                _forwardDirection = _latestServerState.ForwardDirection;
                _leftRightDirection = _latestServerState.LeftRightDirection;
                _grounded = _latestServerState.Grounded;
            }

            base.Update();

            if (!Enabled)
            {
                _forwardDirection = 0;
                _leftRightDirection = 0;
                _jump = false;
                return;
            }

            if ((Input.GetKey(KeyCode.Mouse0) || Input.GetKey(KeyCode.Mouse1)) && DisableCameraMovement == false)
            {
                _rotation.y = transform.localEulerAngles.y + Input.GetAxis("Mouse X") * _angularSpeed * Time.deltaTime;
                transform.localEulerAngles = _rotation;
            }
            _bothMouseButtons = Input.GetKey(KeyCode.Mouse0) && Input.GetKey(KeyCode.Mouse1);

            _leftRightDirection = (Input.GetKey(KeyCode.A) ? -1 : 0) + (Input.GetKey(KeyCode.D) ? 1 : 0);
            _forwardDirection = (Input.GetKey(KeyCode.S) ? -1 : 0) + (Input.GetKey(KeyCode.W) || _bothMouseButtons ? 1 : 0);
            _jump = Input.GetKey(KeyCode.Space);

            if(_interactNpc != null && Input.GetKeyDown(KeyCode.F))
            {
                _interactNpc.Interact();
            }
        }

        private void FixedUpdate()
        {
            _grounded = _characterController.isGrounded;
            if (!_grounded)
            {
                HandleGravity();
                _grounded = _characterController.isGrounded;
                PlayerAnimationController.AnimationUpdate(_grounded, _leftRightDirection, _forwardDirection, ref _animationState, _animator);
            }
            if (_grounded) _yVelocity = 0;

            _timer += Time.deltaTime;

            while (_timer >= _minTimeBetweenTicks)
            {
                _timer -= _minTimeBetweenTicks;
                HandleTick();
                _currentTick++;
                _currentTick = _currentTick % _bufferSize;
            }
        }

        void HandleTick()
        {
            if (!_latestServerState.Equals(default(PlayerState)) &&
                (_lastProcessedState.Equals(default(PlayerState)) ||
                !_latestServerState.Equals(_lastProcessedState)) && AdvancedMovementEnabled)
            {
                HandleServerReconciliation();
            }

            Vector3 rotation = transform.rotation.eulerAngles;

            PlayerInput playerInput = new PlayerInput()
            {
                TickIndex = _currentTick,
                ForwardDirection = _forwardDirection,
                LeftRightDirection = _leftRightDirection,
                Jump = _jump,
                Rotation = new float[3] { rotation.x, rotation.y, rotation.z },
            };
            
            _inputBuffer[_currentTick] = playerInput;
            if (AdvancedMovementEnabled) _stateBuffer[_currentTick] = ProcessMovement(playerInput);
            else _stateBuffer[_currentTick] = BasicMovement(playerInput);

            _message.PlayerInput = playerInput;
            ClientSend.SendUDPData(_message, Client.OverworldServer);
        }

        private PlayerState ProcessMovement(PlayerInput input)
        {
            _moveDirection = transform.forward * _forwardDirection + transform.right * _leftRightDirection;
            if (_forwardDirection != 0 && _leftRightDirection != 0) _moveDirection = _moveDirection / _SQUARE_OF_TWO;
            _moveDirection *= _moveSpeed;

            if (_grounded && _jump)
            {
                _yVelocity = _jumpSpeed;
                _moveDirection.y = _yVelocity;
            }
            else
            {
                _moveDirection.y = 0;
            }

            _characterController.Move(_moveDirection);

            return new PlayerState()
            {
                TickIndex = input.TickIndex,
                Position = new float[3] { (float)Math.Round(transform.position.x, 3),
                    (float)Math.Round(transform.position.y, 3), (float)Math.Round(transform.position.z, 3) },
                LeftRightDirection = input.LeftRightDirection,
                ForwardDirection = input.ForwardDirection,
                Grounded = _grounded,
            };
        }

        private void HandleGravity()
        {
            _yVelocity += _gravity;

            _moveDirection.x = 0;
            _moveDirection.z = 0;
            _moveDirection.y = _yVelocity;
            _characterController.Move(_moveDirection);
        }

        void HandleServerReconciliation()
        {
            _lastProcessedState = _latestServerState;

            int serverStateBufferIndex = _latestServerState.TickIndex;
            _latestVector.x = _latestServerState.Position[0];
            _latestVector.y = _latestServerState.Position[2];

            _bufferVector.x = _stateBuffer[serverStateBufferIndex].Position[0];
            _bufferVector.y = _stateBuffer[serverStateBufferIndex].Position[2];

            float positionError = Vector2.Distance(_latestVector, _bufferVector);

            if (positionError > _reconciliationErrorMargin || 
                Math.Abs(_latestServerState.Position[1] - _stateBuffer[serverStateBufferIndex].Position[1]) > _jumpSpeed * 3)
            {
                Debug.Log($"Reconciliation necessary.");
                _characterController.enabled = false;
                transform.position = new Vector3(_latestVector.x, _latestServerState.Position[1], _latestVector.y);
                _characterController.enabled = true;

                _stateBuffer[serverStateBufferIndex] = _latestServerState;

                int tickToProcess = _latestServerState.TickIndex + 1;
                int bufferIndex = tickToProcess % _bufferSize;

                while (Math.Abs(bufferIndex - _currentTick) > 0)
                {
                    PlayerState statePayload = ProcessMovement(_inputBuffer[bufferIndex]);
                    _stateBuffer[bufferIndex] = statePayload;

                    tickToProcess++;
                    bufferIndex = tickToProcess % _bufferSize;
                }
            }
        }

        private PlayerState BasicMovement(PlayerInput input)
        {
            _characterController.enabled = false;
            transform.position = new Vector3(_latestServerState.Position[0], 
                _latestServerState.Position[1], _latestServerState.Position[2]);
            _characterController.enabled = true;

            return new PlayerState()
            {
                TickIndex = input.TickIndex,
                Position = new float[3] { (float)Math.Round(transform.position.x, 3),
                    (float)Math.Round(transform.position.y, 3), (float)Math.Round(transform.position.z, 3) },
                LeftRightDirection = input.LeftRightDirection,
                ForwardDirection = input.ForwardDirection,
                Grounded = _grounded,
            };
        }

        public void NewServerState(PlayerState state)
        {
            _latestServerState = state;
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
