using System.Collections.Generic;
using UnityEngine;

namespace Advanced_Movement.Individual_Mechanics{
    public class Gravity : Mechanic
    {
        private int _numberOfGroundedCheckPoints;
        private float _radiusOfGroundedCheckPoints, _groundCheckCastDistance, _gravity, _fallMultiplier;

        private List<Vector3> _gravityCheckersList;
        private float _fallSpeed = 0;

        private LayerMask _groundMask;

        private void Update(){
            CheckGrounded();
        }

        protected override void CheckIfEnabled(){
            enabled = _pm.mechanicsDictionary[PlayerMovement.Mechanics.Gravity.ToString()];
        }

        protected override void VariablesSetUp(){
            _numberOfGroundedCheckPoints = _pm.numberOfGroundedCheckPoints;
            _radiusOfGroundedCheckPoints = _pm.radiusOfGroundedCheckPoints;
            _groundCheckCastDistance = _pm.groundCheckCastDistance;
            _gravity = _pm.gravity;
            _fallMultiplier = _pm.fallMultiplier;

            _groundMask = _pm.groundMask;
            
            CreateGravityCheckers();
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
                Vector3 pointPosition = new Vector3(Mathf.Cos(currentAngleInRad), 0, Mathf.Sin(currentAngleInRad)) * _radiusOfGroundedCheckPoints;
        
                _gravityCheckersList.Add(pointPosition);
            
                currentAngle += gradPerSegment;
            }
        }
        
        private void CheckGrounded()
        {
            foreach (var point in _gravityCheckersList)
            {
                Vector3 pointRelativeToPlayer = transform.position + point;
                if (Physics.Raycast(pointRelativeToPlayer, Vector3.down, _groundCheckCastDistance, _groundMask))
                {
                    if (_fallSpeed != 0)
                    {
                        _fallSpeed = 0;
                    }
                    return;
                }
            }
            _fallSpeed -= _gravity * Time.deltaTime;
            _pm.controller.Move(Vector3.up * (_fallSpeed * _fallMultiplier * Time.deltaTime));
        }
        
        #endregion
    }
}
