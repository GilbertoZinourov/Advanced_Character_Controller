using System;
using System.Collections;
using UnityEngine;

namespace Advanced_Movement.Individual_Mechanics{
    public class EightDirMovement : Mechanic{
        private Transform _camTransform;

        private float _walkSpeed,
            _runSpeed,
            _mouseSensitivity,
            _minVerticalLookClampValue,
            _maxVerticalLookClampValue,
            _currentMovementSpeed = 0,
            _xRotation = 0f,
            _crouchTime,
            _slideTime;

        public bool _invertedX, _invertedY, _isRunning, _canCrouch, _canSlide, _isCrouched, _isInCoroutine;
        private LayerMask _groundMask;

        private PlayerMovement.PressMode _runMode, _crouchMode;

        protected override void OnEnable(){
            base.OnEnable();

            _camTransform = Camera.main.transform;

            _pm.OnMovementInput += Movement;
            _pm.OnRunningInputPressed += Running;
            _pm.OnRunningInputReleased += RunningOff;
            _pm.OnMouseInput += LookAround;
            _pm.OnCrouchInputPressed += Crouch;
            _pm.OnCrouchInputReleased += CrouchOff;
        }

        protected override void OnDisable(){
            base.OnDisable();

            _pm.OnMovementInput -= Movement;
            _pm.OnRunningInputPressed -= Running;
            _pm.OnRunningInputReleased -= RunningOff;
            _pm.OnMouseInput -= LookAround;
            _pm.OnCrouchInputPressed -= Crouch;
            _pm.OnCrouchInputReleased -= CrouchOff;
        }

        private void Update(){
            Move();
        }

        protected override void VariablesSetUp(){
            _walkSpeed = _pm.walkSpeed;
            _runSpeed = _pm.runSpeed;
            _mouseSensitivity = _pm.mouseSensitivity;
            _minVerticalLookClampValue = _pm.minVerticalLookClampValue;
            _maxVerticalLookClampValue = _pm.maxVerticalLookClampValue;

            _invertedX = _pm.invertedX;
            _invertedY = _pm.invertedY;

            _runMode = _pm.runMode;
            _groundMask = _pm.groundMask;

            _canCrouch = _pm.canCrouch;
            _crouchMode = _pm.crouchMode;
            _canSlide = _pm.canSlide;
            _slideTime = _pm.slideTime;

            if (_canCrouch){
                _crouchMode = _pm.crouchMode;
                _crouchTime = _pm.crouchTime;
                _canSlide = _pm.canSlide;
            }
            else{
                _canSlide = false;
            }
        }

        protected override void CheckIfEnabled(){
            enabled = _pm.mechanicsDictionary[PlayerMovement.Mechanics.Base8DirMovement.ToString()];
        }

        private void Running(){
            switch (_runMode){
                case PlayerMovement.PressMode.Toggle:
                    _isRunning = !_isRunning;
                    break;
                case PlayerMovement.PressMode.Hold:
                    _isRunning = true;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void RunningOff(){
            _isRunning = false;
        }

        private void Crouch(){
            if (!_canCrouch) return;
            switch (_crouchMode){
                case PlayerMovement.PressMode.Toggle:
                    _isCrouched = !_isCrouched;
                    break;
                case PlayerMovement.PressMode.Hold:
                    _isCrouched = true;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            if (!_isInCoroutine){
                StartCoroutine(CrouchCoroutine());
                StartCoroutine(SlideCoroutine());
            }
        }

        private void CrouchOff(){
        }

        private void Movement(Vector2 input){
            float targetSpeed;
            if (input.magnitude <= .01f){
                targetSpeed = 0;
                if (_runMode == PlayerMovement.PressMode.Toggle){
                    RunningOff();
                }

                _pm.ChangePlayerState(PlayerMovement.PlayerStates.Idle);
            }
            else{
                if (_isRunning){
                    targetSpeed = _runSpeed;
                    _pm.ChangePlayerState(PlayerMovement.PlayerStates.Running);
                }
                else{
                    targetSpeed = input.magnitude * _walkSpeed;
                    _pm.ChangePlayerState(PlayerMovement.PlayerStates.Walking);
                }
            }

            _currentMovementSpeed = targetSpeed;

            _pm.MovementDirection =
                Vector3.Normalize(new Vector3(_camTransform.forward.x, 0, _camTransform.forward.z)) * input.y +
                _camTransform.right * input.x;
        }

        private void LookAround(Vector2 input){
            if (_invertedX){
                input.x *= -1;
            }

            if (_invertedY){
                input.y *= -1;
            }

            input *= (_mouseSensitivity * Time.deltaTime);
            _xRotation -= input.y;
            _xRotation = Mathf.Clamp(_xRotation, _minVerticalLookClampValue, _maxVerticalLookClampValue);

            switch (_pm.desiredPerspective){
                case PlayerMovement.FirstOrThird.First:
                    transform.Rotate(Vector3.up * input.x);
                    _pm.firstPersonCameraPosition.localRotation = Quaternion.Euler(_xRotation, 0, 0);
                    break;
                case PlayerMovement.FirstOrThird.Third:
                    transform.Rotate(Vector3.up * input.x);
                    _pm.centerOfRotation.localRotation = Quaternion.Euler(_xRotation, 0, 0);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        // Applica il movimento al CharacterController tenendo in considerazione le irregolarità del terreno
        private void Move(){
            if (_pm.currentState == PlayerMovement.PlayerStates.Idle
                || _pm.currentState == PlayerMovement.PlayerStates.Walking
                || _pm.currentState == PlayerMovement.PlayerStates.Running){
                RaycastHit hit;
                if (Physics.Raycast(transform.position, Vector3.down, out hit, _groundMask)){
                    Vector3 right = new Vector3(_pm.MovementDirection.z, 0, -_pm.MovementDirection.x);
                    _pm.MovementDirection = Vector3.Cross(right, hit.normal).normalized;
                }

                _pm.MovementDirection *= _currentMovementSpeed;
                if (_pm.MovementDirection.magnitude <= .01f && _runMode == PlayerMovement.PressMode.Toggle){
                    RunningOff();
                }

                _pm.controller.Move(_pm.MovementDirection * Time.deltaTime);
            }
        }

        private IEnumerator SlideCoroutine(){
            _isInCoroutine = true;
            if (_canSlide && _pm.currentState == PlayerMovement.PlayerStates.Running){
                _pm.currentState = PlayerMovement.PlayerStates.Sliding;
            
                float time = 0f;
                var startVector = _pm.MovementDirection;
                while (_pm.MovementDirection != Vector3.zero){
                    _pm.MovementDirection = Vector3.Lerp(startVector, Vector3.zero, time / _slideTime);
                    time += Time.deltaTime;
                    yield return null;
                }
            }

            StartCoroutine(CrouchCoroutine());
        }

        private IEnumerator CrouchCoroutine(){
            _isInCoroutine = true;
            // if (_canSlide && _pm.currentState == PlayerMovement.PlayerStates.Running){
            //     _pm.currentState = PlayerMovement.PlayerStates.Sliding;
            //
            //     float time = 0f;
            //     var startVector = _pm.MovementDirection;
            //     while (time <= _slideTime){
            //         _pm.MovementDirection = Vector3.Lerp(startVector, Vector3.zero, time / _slideTime);
            //         time += Time.deltaTime;
            //         yield return null;
            //     }
            // }

            Vector3 targetCameraPosition, targetControllerPosition;
            float targetControllerHeight;

            if (!_isCrouched){
                targetCameraPosition =
                    _camTransform.parent.localPosition + Vector3.up * .85f; // HardCoded non ideale...
                targetControllerHeight = 2;
                targetControllerPosition = Vector3.zero;
            }
            else{
                targetCameraPosition =
                    _camTransform.parent.localPosition - Vector3.up * .85f; // HardCoded non ideale...
                targetControllerHeight = 1;
                targetControllerPosition = Vector3.up * -.5f;
            }

            var time = 0f;
            Vector3 startCameraPosition = _camTransform.parent.localPosition;
            float startControllerHeight = _pm.controller.height;
            Vector3 startControllerPosition = _pm.controller.center;

            while (_camTransform.parent.localPosition != targetCameraPosition){
                var speed = time / _crouchTime;
                _camTransform.parent.localPosition =
                    Vector3.Lerp(startCameraPosition, targetCameraPosition, speed);
                _pm.controller.height = Mathf.Lerp(startControllerHeight, targetControllerHeight, speed);
                _pm.controller.center = Vector3.Lerp(startControllerPosition, targetControllerPosition, speed);

                time += Time.deltaTime;

                yield return null;
            }

            _isInCoroutine = false;
            yield return null;
        }
    }
}