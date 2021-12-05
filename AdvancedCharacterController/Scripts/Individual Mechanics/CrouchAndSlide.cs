//    Advanced Character Controller
//    Copyright (C) 2021  Gilberto Zinourov
//
//    This program is free software: you can redistribute it and/or modify
//    it under the terms of the GNU General Public License as published by
//    the Free Software Foundation, either version 3 of the License, or
//    (at your option) any later version.
//
//    This program is distributed in the hope that it will be useful,
//    but WITHOUT ANY WARRANTY; without even the implied warranty of
//    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//    GNU General Public License for more details.
//
//    You should have received a copy of the GNU General Public License
//    along with this program.  If not, see <https://www.gnu.org/licenses/>.



using System;
using System.Collections;
using UnityEngine;

namespace Advanced_Movement.Individual_Mechanics{
    public class CrouchAndSlide : Mechanic{

        private bool _canSlide;
        private float _crouchTime, _slideTime;

        private int _slideEndingSpeedPercentage; 
        
        private LayerMask _groundMask;
        private AdvancedMovement.PressMode _crouchMode;

        private bool _isCrouching;
        private bool _isInCoroutine;

        private Transform _camPivot;

        private float _startHeight = 2f, _targetHeight = 1f;
        private Vector3 _startPos = new Vector3(0, 0, 0), _targetPos = new Vector3(0, -.5f, 0);
        private Vector3 _startCamPos, _targetCamPos;

        private EightDirMovement _baseMovement;
        private float _runSpeed;
        
        
        protected override void OnEnable(){
            base.OnEnable();
            
            _pm.OnCrouchInputPressed += Crouch;
            _pm.OnCrouchInputReleased += CrouchOff;
            _pm.OnJumpInputPressed += CrouchOff;
            _pm.OnRunningInputPressed += CrouchOff;
        }

        protected override void OnDisable(){
            base.OnDisable();
            
            _pm.OnCrouchInputPressed -= Crouch;
            _pm.OnCrouchInputReleased -= CrouchOff;
            _pm.OnJumpInputPressed -= CrouchOff;
            _pm.OnRunningInputPressed -= CrouchOff;
        }

        protected override void CheckIfEnabled(){
            enabled = _pm.mechanicsDictionary[AdvancedMovement.Mechanics.CrouchAndSlide.ToString()];
        }

        protected override void VariablesSetUp(){
            _canSlide = _pm.canSlide;
            _crouchTime = _pm.crouchTime;
            _slideTime = _pm.slideTime;
            _groundMask = _pm.groundMask;
            _crouchMode = _pm.crouchMode;
            _slideEndingSpeedPercentage = _pm.slideEndingSpeedPercentage;
            _runSpeed = _pm.runSpeed;

            _baseMovement = _pm.mechanicsDictionary[AdvancedMovement.Mechanics.Base8DirMovement.ToString()] ? GetComponent<EightDirMovement>() : null;
            
            _camPivot = Camera.main.transform.parent;
            
            if (_isCrouching){
                _targetCamPos = _camPivot.localPosition;
                _startCamPos = _camPivot.localPosition + new Vector3(0, .85f, 0);
            }
            else{
                _targetCamPos = _camPivot.localPosition - new Vector3(0, .85f, 0);
                _startCamPos = _camPivot.localPosition;
            }
        }

        private void Crouch(){
            if (_isInCoroutine || _pm.currentState == AdvancedMovement.PlayerStates.Sliding) return;
            switch(_crouchMode){
                case AdvancedMovement.PressMode.Toggle:
                    _isCrouching = !_isCrouching;
                    break;
                case AdvancedMovement.PressMode.Hold:
                    _isCrouching = true;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            if (_isCrouching){
                StartCoroutine(CrouchUnCrouch(
                    _startPos, _startHeight, _startCamPos,
                    _targetPos, _targetHeight, _targetCamPos));
                if (_canSlide && _pm.currentState == AdvancedMovement.PlayerStates.Running){
                    StartCoroutine(Slide());
                }
            }
            else{
                StartCoroutine(CrouchUnCrouch(
                    _targetPos, _targetHeight, _targetCamPos, 
                    _startPos, _startHeight, _startCamPos));
            }
        }

        private IEnumerator Slide(){
            if (_baseMovement){
                _baseMovement.RunningOff();
            }
            
            Vector3 start = _pm.MovementDirection * _runSpeed;
            Vector3 end = (start / 100) * _slideEndingSpeedPercentage;
            
            _pm.ChangePlayerState(AdvancedMovement.PlayerStates.Sliding);

            float time = 0;

            while(time < _slideTime){
                if (_pm.currentState == AdvancedMovement.PlayerStates.InAir) break;
                time += Time.deltaTime;
                _pm.MovementDirection = Vector3.Lerp(start, end, time/_slideTime);
                _pm.controller.Move(_pm.MovementDirection * Time.deltaTime);
                yield return null;
            }
            
            _pm.ChangePlayerState(AdvancedMovement.PlayerStates.Idle);
            yield return null;
            
        }

        public void CrouchOff(){
            if (_isInCoroutine) return;
            _isCrouching = false;
            StartCoroutine(CrouchUnCrouch(
                _targetPos, _targetHeight, _targetCamPos, 
                _startPos, _startHeight, _startCamPos));
        }

        private IEnumerator CrouchUnCrouch(
            Vector3 startPos, float startHeight, Vector3 startCamPos, 
            Vector3 targetPos, float targetHeight, Vector3 targetCamPos){
            
            _isInCoroutine = true;
            
            float time = 0;
            while (_pm.controller.center != targetPos){
                var speed = time / _crouchTime;
                _pm.controller.center = Vector3.Lerp(startPos, targetPos, speed);
                _camPivot.localPosition = Vector3.Lerp(startCamPos, targetCamPos, speed);
                _pm.controller.height = Mathf.Lerp(startHeight, targetHeight, speed);
                time += Time.deltaTime;
                yield return null;
            }

            _isInCoroutine = false;
        }
    }
}
