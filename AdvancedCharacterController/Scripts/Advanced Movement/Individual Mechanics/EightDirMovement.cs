// MIT License
//
// Copyright (c) 2021 Gilberto Zinourov
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.



using System;
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
            _xRotation = 0f;

        private bool _invertedX, _invertedY, _isRunning;
        private LayerMask _groundMask;

        private AdvancedMovement.PressMode _runMode;

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
        }

        protected override void CheckIfEnabled(){
            enabled = _pm.mechanicsDictionary[AdvancedMovement.Mechanics.Base8DirMovement.ToString()];
        }

        private void Running(){
            if (_pm.currentState == AdvancedMovement.PlayerStates.Sliding) return;
            switch (_runMode){
                case AdvancedMovement.PressMode.Toggle:
                    _isRunning = !_isRunning;
                    break;
                case AdvancedMovement.PressMode.Hold:
                    _isRunning = true;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            if (_isRunning && _pm.mechanicsDictionary[AdvancedMovement.Mechanics.CrouchAndSlide.ToString()]){
                GetComponent<CrouchAndSlide>().CrouchOff();
            }
        }

        public void RunningOff(){
            _isRunning = false;
        }

        private void Movement(Vector2 input){
            if (_pm.currentState == AdvancedMovement.PlayerStates.Sliding ||
                _pm.currentState == AdvancedMovement.PlayerStates.InAir || 
                _pm.currentState == AdvancedMovement.PlayerStates.Mantling ||
                _pm.currentState == AdvancedMovement.PlayerStates.WallClimbing ||
                _pm.currentState == AdvancedMovement.PlayerStates.WallRunning) return;
            float targetSpeed;
            if (input.magnitude <= .01f){
                targetSpeed = 0;
                if (_runMode == AdvancedMovement.PressMode.Toggle){
                    RunningOff();
                }

                _pm.ChangePlayerState(AdvancedMovement.PlayerStates.Idle);
            }
            else{
                if (_isRunning){
                    targetSpeed = _runSpeed;
                    _pm.ChangePlayerState(AdvancedMovement.PlayerStates.Running);
                }
                else{
                    targetSpeed = input.magnitude * _walkSpeed;
                    _pm.ChangePlayerState(AdvancedMovement.PlayerStates.Walking);
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
                case AdvancedMovement.FirstOrThird.First:
                    _pm.firstPersonCameraPosition.localRotation = Quaternion.Euler(_xRotation, 0, 0);
                    break;
                case AdvancedMovement.FirstOrThird.Third:
                    _pm.centerOfRotation.localRotation = Quaternion.Euler(_xRotation, 0, 0);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            if (_pm.currentState == AdvancedMovement.PlayerStates.Mantling || 
                _pm.currentState == AdvancedMovement.PlayerStates.WallRunning || 
                _pm.currentState == AdvancedMovement.PlayerStates.WallClimbing) return;
            
            transform.Rotate(Vector3.up * input.x);
        }

        // Applica il movimento al CharacterController tenendo in considerazione le irregolarità del terreno
        private void Move(){
            if (_pm.currentState == AdvancedMovement.PlayerStates.Sliding ||
                _pm.currentState == AdvancedMovement.PlayerStates.InAir ||
                _pm.currentState == AdvancedMovement.PlayerStates.Mantling ||
                _pm.currentState == AdvancedMovement.PlayerStates.WallClimbing ||
                _pm.currentState == AdvancedMovement.PlayerStates.WallRunning) return;
            RaycastHit hit;
            if (Physics.Raycast(transform.position +_pm.controller.center, Vector3.down, out hit, _groundMask)){
                Vector3 right = new Vector3(_pm.MovementDirection.z, 0, -_pm.MovementDirection.x);
                _pm.MovementDirection = Vector3.Cross(right, hit.normal).normalized;
            }

            _pm.MovementDirection *= _currentMovementSpeed;
            if (_pm.MovementDirection.magnitude <= .01f && _runMode == AdvancedMovement.PressMode.Toggle){
                RunningOff();
            }

            _pm.controller.Move(_pm.MovementDirection * Time.deltaTime);
        }
    }
}