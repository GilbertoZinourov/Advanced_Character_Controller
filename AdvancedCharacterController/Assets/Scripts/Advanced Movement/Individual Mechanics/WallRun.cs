using System.Collections;
using UnityEngine;

namespace Advanced_Movement.Individual_Mechanics{
    public class WallRun : Mechanic{
        public int _checkersPerSide = 3;
        public float _distance = 1f;

        public LayerMask _groundMask;

        public int _onWall = 0;

        public float wallRunningSpeed;

        [Range(1, 10)] public float lookAdjustSpeed = 1;

        public bool _inCoroutine, _inLoop, _canWallJump = true;

        public float verticalAdjust, jumpForce = 5;


        private void Update(){
            CheckWall();
        }

        protected override void OnEnable(){
            base.OnEnable();

            _pm.OnMovementInput += InputPressed;
            _pm.OnJumpInputPressed += WallJump;
        }

        protected override void OnDisable(){
            base.OnDisable();

            _pm.OnMovementInput -= InputPressed;
            _pm.OnJumpInputPressed -= WallJump;
        }

        private void WallJump(){
            if (!_canWallJump || _pm.currentState != PlayerMovement.PlayerStates.WallRunning) return;

            Vector3 jumpVector = (_pm.MovementDirection.normalized - transform.right + transform.up).normalized *
                                 jumpForce;
            _pm.ChangePlayerState(PlayerMovement.PlayerStates.InAir);
            _pm.MovementDirection = jumpVector;
        }

        private void InputPressed(Vector2 obj){
            if (obj.y < .5f){
                _inLoop = false;
                _onWall = 0;
                return;
            }
            if (!_inCoroutine && _onWall != 0){ 
                StartCoroutine(WallRunning());
            }
            
        }

        private void CheckWall(){
            if (_pm.currentState != PlayerMovement.PlayerStates.InAir) return;

            float gradPerSegment = 180f / (_checkersPerSide + 1);
            float currentAngle = 0f;

            for (int i = 0; i < _checkersPerSide; i++){
                currentAngle += gradPerSegment;
                var currentAngleInRad = currentAngle * Mathf.Deg2Rad;
                Vector3 pointPos = transform.right * Mathf.Sin(currentAngleInRad) +
                                   transform.forward * Mathf.Cos(currentAngleInRad);
                if (Physics.Raycast(transform.position, pointPos, _distance, _groundMask)){
                    //_pm.ChangePlayerState(PlayerMovement.PlayerStates.WallRunning);
                    _onWall = 1;
                }

                pointPos = -transform.right * Mathf.Sin(currentAngleInRad) +
                           transform.forward * Mathf.Cos(currentAngleInRad);
                if (Physics.Raycast(transform.position, pointPos, _distance, _groundMask)){
                    //_pm.ChangePlayerState(PlayerMovement.PlayerStates.WallRunning);
                    _onWall = 2;
                }
            }
        }

        private IEnumerator WallRunning(){
            _inCoroutine = true;
            _inLoop = true;
            _pm.ChangePlayerState(PlayerMovement.PlayerStates.WallRunning);

            RaycastHit hit;
            
            //print("Start");

            while (_inLoop && _pm.currentState == PlayerMovement.PlayerStates.WallRunning){
                if (Physics.Raycast(transform.position, transform.right, out hit, _distance * 2, _groundMask) ||
                    Physics.Raycast(transform.position, -transform.right, out hit, _distance * 2, _groundMask)){
                    Vector3 secondVector = Vector3.zero;
                    if (_onWall == 1){
                        secondVector = -transform.up;
                    }
                    else{
                        secondVector = transform.up;
                    }

                    _pm.MovementDirection = Vector3.Cross(hit.normal, secondVector).normalized * wallRunningSpeed;
                    _pm.controller.Move(_pm.MovementDirection * Time.deltaTime);
                    transform.forward = Vector3.Lerp(transform.forward, _pm.MovementDirection.normalized,
                        .1f / lookAdjustSpeed);
                }
                else{
                    break;
                }

                yield return null;
            }

            _onWall = 0;
            if (_pm.currentState == PlayerMovement.PlayerStates.WallRunning){
                _pm.ChangePlayerState(PlayerMovement.PlayerStates.InAir);
            }

            //print("Finish");
            
            _inCoroutine = false;
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