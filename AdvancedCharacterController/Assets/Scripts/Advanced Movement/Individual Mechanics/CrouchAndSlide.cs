using System;
using System.Collections;
using UnityEngine;

namespace Advanced_Movement.Individual_Mechanics{
    public class CrouchAndSlide : Mechanic{

        private bool _canSlide;
        private float _crouchTime, _slideTime;
        private LayerMask _groundMask;
        private PlayerMovement.PressMode _crouchMode;

        private bool _isCrouching;
        private bool _isInCoroutine;

        private Transform _camPivot;

        private float _startHeight = 2f, _targetHeight = 1f;
        private Vector3 _startPos = new Vector3(0, 0, 0), _targetPos = new Vector3(0, -.5f, 0);
        private Vector3 _startCamPos, _targetCamPos;
        
        
        protected override void OnEnable(){
            base.OnEnable();
            
            _pm.OnCrouchInputPressed += Crouch;
            _pm.OnCrouchInputReleased += CrouchOff;
        }

        protected override void OnDisable(){
            base.OnDisable();
            
            _pm.OnCrouchInputPressed -= Crouch;
            _pm.OnCrouchInputReleased -= CrouchOff;
        }

        protected override void CheckIfEnabled(){
            enabled = _pm.mechanicsDictionary[PlayerMovement.Mechanics.CrouchAndSlide.ToString()];
        }

        protected override void VariablesSetUp(){
            _canSlide = _pm.canSlide;
            _crouchTime = _pm.crouchTime;
            _slideTime = _pm.slideTime;
            _groundMask = _pm.groundMask;
            _crouchMode = _pm.crouchMode;

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
            if (_isInCoroutine) return;
            switch(_crouchMode){
                case PlayerMovement.PressMode.Toggle:
                    _isCrouching = !_isCrouching;
                    break;
                case PlayerMovement.PressMode.Hold:
                    _isCrouching = true;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            if (_isCrouching){
                StartCoroutine(CrouchUnCrouch(
                    _startPos, _startHeight, _startCamPos,
                    _targetPos, _targetHeight, _targetCamPos));
            }
            else{
                StartCoroutine(CrouchUnCrouch(
                    _targetPos, _targetHeight, _targetCamPos, 
                    _startPos, _startHeight, _startCamPos));
            }
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
