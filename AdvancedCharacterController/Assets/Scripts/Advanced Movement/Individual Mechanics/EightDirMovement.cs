using System;
using System.Collections.Generic;
using UnityEngine;

namespace Advanced_Movement.Individual_Mechanics{
    public class EightDirMovement : Mechanic{

        private Transform _camTransform;

        private float _walkSpeed,
            _runSpeed,
            _mouseSensitivity,
            _minVerticalLookClampValue,
            _maxVerticalLookClampValue,
            _currentMovementSpeed = 0, _xRotation = 0f;
        private bool _invertedX, _invertedY, _isRunning;
        private LayerMask _groundMask;
        private Vector3 _finalDir;

        private PlayerMovement.RunMode _runMode;
        
        protected override void OnEnable(){
            base.OnEnable();
            
            _camTransform = Camera.main.transform;
            
            _pm.OnMovementInput += Movement;
            _pm.OnRunningInputPressed += Running;
            _pm.OnRunningInputReleased += RunningOff;
            _pm.OnMouseInput += LookAround;
        }

        protected override void OnDisable(){
            base.OnDisable();
            
            _pm.OnMovementInput -= Movement;
            _pm.OnRunningInputPressed -= Running;
            _pm.OnRunningInputReleased -= RunningOff;
            _pm.OnMouseInput -= LookAround;
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
        }
        
        protected override void CheckIfEnabled(){
            enabled = _pm.mechanicsDictionary[PlayerMovement.Mechanics.Base8DirMovement.ToString()];
        }

        private void Running(){
            switch(_runMode){
                case PlayerMovement.RunMode.Toggle:
                    _isRunning = !_isRunning;
                    break;
                case PlayerMovement.RunMode.Hold:
                    _isRunning = false;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        
        private void RunningOff(){
            _isRunning = false;
        }

        private void Movement(Vector2 input){
            float targetSpeed;
            if (input.magnitude <= .01f)
            {
                targetSpeed = 0;
            }
            else
            {
                if (_isRunning)
                {
                    targetSpeed = _runSpeed;
                }
                else
                {
                    targetSpeed = input.magnitude * _walkSpeed;
                }
            }
            
            _currentMovementSpeed = targetSpeed;
            
            Vector3 movementDirection = Vector3.Normalize(new Vector3(_camTransform.forward.x, 0, _camTransform.forward.z)) * input.y + 
                                        _camTransform.right * input.x;
            RaycastHit hit;
            if (Physics.Raycast(transform.position, Vector3.down, out hit, _groundMask))
            {
                Vector3 right = new Vector3(movementDirection.z, 0, -movementDirection.x);
                _finalDir = right;
                movementDirection = Vector3.Cross(right , hit.normal).normalized;
            }
            
            //_finalDir = movementDirection;
            movementDirection *= _currentMovementSpeed;
            _pm.controller.Move(movementDirection * Time.deltaTime);
            //Debug.Log("Moving with speed: " + _currentMovementSpeed);
        }
        
        private void LookAround(Vector2 input)
        {
            if (_invertedX)
            {
                input.x *= -1;
            }
        
            if (_invertedY)
            {
                input.y *= -1;
            }
        
            input *= (_mouseSensitivity * Time.deltaTime);
            _xRotation -= input.y;
            _xRotation = Mathf.Clamp(_xRotation, _minVerticalLookClampValue, _maxVerticalLookClampValue);
        
            switch (_pm.desiredPerspective)
            {
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
    }
}
