using System;
using UnityEngine;

namespace Advanced_Movement.Individual_Mechanics{
    public class WallRun : Mechanic{
        public int _checkersPerSide = 3;
        public float _distance = 1f;

        public LayerMask _groundMask;

        private void Update(){
            CheckRightWall();
        }

        private void CheckRightWall(){
            float gradPerSegment = 180f / (_checkersPerSide + 1);
            float currentAngle = 0f;

            for (int i = 0; i < _checkersPerSide; i++){
                currentAngle += gradPerSegment;
                var currentAngleInRad = currentAngle * Mathf.Deg2Rad;
                Vector3 pointPos = transform.right * Mathf.Sin(currentAngleInRad) +
                                    transform.forward * Mathf.Cos(currentAngleInRad);
                if (Physics.Raycast(transform.position, pointPos, _distance, _groundMask)){
                    print("wall at " + i);
                }
            }
        }


        private void OnDrawGizmos(){
            float gradPerSegment = 180f / (_checkersPerSide + 1);
            float currentAngle = 0f;
            
            Gizmos.color = Color.magenta;
            
            for (int i = 0; i < _checkersPerSide; i++){
                currentAngle += gradPerSegment;
                var currentAngleInRad = currentAngle * Mathf.Deg2Rad;
                Vector3 pointPos = (transform.right * Mathf.Sin(currentAngleInRad) +
                                                         transform.forward * Mathf.Cos(currentAngleInRad)).normalized * _distance;
                Gizmos.DrawLine(transform.position, transform.position + pointPos);
            }
        }
    }
}
