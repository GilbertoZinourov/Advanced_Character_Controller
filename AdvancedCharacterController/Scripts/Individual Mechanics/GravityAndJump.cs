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



using System.Collections.Generic;
using UnityEngine;

namespace Advanced_Movement.Individual_Mechanics{
    public class GravityAndJump : Mechanic
    {
        private int _numberOfGroundedCheckPoints, _numberOfPossibleJumps;
        private float _jumpsRemaining;
        private float _radiusOfGroundedCheckPoints, _groundCheckCastDistance, _gravity, _fallMultiplier, _jumpForce;
        private bool _isInAir, _canJump;

        private List<Vector3> _gravityCheckersList;
        private float _fallSpeed = 0;

        private LayerMask _groundMask;

        private Transform _camTransform;
        private Vector3 _targetXZ;
        private float _fallOffSpeed, _maxInAirSpeed;

        private CrouchAndSlide _crouchScript;
        private EightDirMovement _dirMovementScript;

        private Vector3 _playerCenter;
        private float _ceilingCheckCastDistance;

        private void Update(){
            CheckGrounded();
            CheckCeiling();
        }

        protected override void OnEnable(){
            base.OnEnable();

            _pm.OnJumpInputPressed += Jump;
            _pm.OnMovementInput += Movement;
        }

        protected override void OnDisable(){
            base.OnDisable();

            _pm.OnJumpInputPressed -= Jump;
            _pm.OnMovementInput -= Movement;
        }

        protected override void CheckIfEnabled(){
            enabled = _pm.mechanicsDictionary[AdvancedMovement.Mechanics.GravityAndJump.ToString()];
        }

        protected override void VariablesSetUp(){
            _numberOfGroundedCheckPoints = _pm.numberOfGroundedCheckPoints;
            _radiusOfGroundedCheckPoints = _pm.radiusOfGroundedCheckPoints;
            _groundCheckCastDistance = _pm.groundCheckCastDistance;
            _gravity = _pm.gravity;
            _fallMultiplier = _pm.fallMultiplier;

            _groundMask = _pm.groundMask;
            _canJump = _pm.canJump;

            _numberOfPossibleJumps = _pm.numberOfPossibleJumps;
            _jumpForce = _pm.jumpForce;

            _jumpsRemaining = _numberOfPossibleJumps;

            _playerCenter = _pm.centerOfPlayer;
            _ceilingCheckCastDistance = _pm.ceilingCheckCastDistance;
            
            CreateGravityCheckers();

            _camTransform = Camera.main.transform;

            _fallOffSpeed = _pm.fallOffSpeed;
            _maxInAirSpeed = _pm.maxInAirSpeed;

            _crouchScript = GetComponent<CrouchAndSlide>();
            _dirMovementScript = GetComponent<EightDirMovement>();
        }
        
        #region Mechanics Methods
        
        private void CreateGravityCheckers()
        {
            _gravityCheckersList = new List<Vector3>();
        
            float gradPerSegment = (float) 360 / _numberOfGroundedCheckPoints;
            float currentAngle = 0f;
            for (int i = 0; i < _numberOfGroundedCheckPoints; i++)
            {
                float currentAngleInRad = currentAngle * Mathf.Deg2Rad;
                Vector3 pointPosition = _playerCenter + new Vector3(Mathf.Cos(currentAngleInRad), 0, Mathf.Sin(currentAngleInRad)) * _radiusOfGroundedCheckPoints;
        
                _gravityCheckersList.Add(pointPosition);
            
                currentAngle += gradPerSegment;
            }
        }
        
        private void CheckGrounded(){
            if (_pm.currentState == AdvancedMovement.PlayerStates.Mantling ||
                _pm.currentState == AdvancedMovement.PlayerStates.WallClimbing ||
                _pm.currentState == AdvancedMovement.PlayerStates.WallRunning){
                _fallSpeed = 0;
                return;
            }
            
            if (_fallSpeed <= 0){
                foreach (var point in _gravityCheckersList){
                    Vector3 pointRelativeToPlayer = transform.position + point;
                    if (Physics.Raycast(pointRelativeToPlayer, Vector3.down, _groundCheckCastDistance, _groundMask)){
                        _fallSpeed = 0;
                        _jumpsRemaining = _numberOfPossibleJumps;
                        _isInAir = false;
                        if (_pm.currentState == AdvancedMovement.PlayerStates.InAir){
                            _pm.ChangePlayerState(AdvancedMovement.PlayerStates.Idle);
                        }

                        return;
                    }
                }
            }
            _pm.ChangePlayerState(AdvancedMovement.PlayerStates.InAir);
            _fallSpeed -= _gravity * _fallMultiplier * Time.deltaTime;
            
            // Add a Lerp to zero on the (x, z) plane?

            Vector2 xzPlane = new Vector2(_pm.MovementDirection.x, _pm.MovementDirection.z);

            float fallOffSpeed = _fallOffSpeed / 100;
            
            if (xzPlane.magnitude > _maxInAirSpeed){
                fallOffSpeed = _fallOffSpeed / 1000;
            }

            xzPlane = Vector2.Lerp(xzPlane, new Vector2(_targetXZ.x, _targetXZ.z), fallOffSpeed);
            
            Vector3 falloffVector = new Vector3(xzPlane.x, _fallSpeed, xzPlane.y);
            _pm.MovementDirection = falloffVector;

            _pm.controller.Move(_pm.MovementDirection * Time.deltaTime);

            if (!_isInAir){
                _jumpsRemaining--;
                _isInAir = true;
                
                // // Manage other movement in the moment of the jump
                // _crouchScript.CrouchOff();
                // _dirMovementScript.RunningOff();
            }
        }

        private void CheckCeiling(){
            if (_pm.currentState != AdvancedMovement.PlayerStates.InAir) return;
            for (int i = 0; i < _gravityCheckersList.Count; i++){
                if (_fallSpeed > 0 && Physics.Raycast(transform.position + _gravityCheckersList[i], Vector3.up,
                    _ceilingCheckCastDistance)){
                    _fallSpeed = 0;
                    break;
                }
            }
        }

        private void Movement(Vector2 input){
            if (_pm.currentState != AdvancedMovement.PlayerStates.InAir) return;
            float targetSpeed;
            if (input.magnitude <= .01f){
                targetSpeed = 0;
            }
            else{
                targetSpeed = _maxInAirSpeed;
            }
            
            _targetXZ =
                Vector3.Normalize(new Vector3(_camTransform.forward.x, 0, _camTransform.forward.z)) * input.y +
                _camTransform.right * input.x;
            _targetXZ *= targetSpeed;
        }
        
        private void Jump(){
            if (_canJump && _jumpsRemaining > 0){
                _fallSpeed = _jumpForce;
                _pm.MovementDirection = new Vector3(_pm.MovementDirection.x, _fallSpeed, _pm.MovementDirection.z);
                if (_isInAir){
                    _jumpsRemaining--;
                }
            }
        }
        
        #endregion
    }
}
