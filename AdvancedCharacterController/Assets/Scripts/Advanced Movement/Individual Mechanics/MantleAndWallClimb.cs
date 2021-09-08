using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Advanced_Movement.Individual_Mechanics{
    public class MantleAndWallClimb : Mechanic{
        private List<Vector3> _checkers;

        public int _numOfCheckers = 4;
        public float _playerHeight = 2;
        public Vector3 _centerOfPlayer = Vector3.zero;
        public float _distance = .5f;

        public LayerMask ground;

        public bool isMantling = false, isWallClimbing = false;

        private float _distanceBetweenCheckers = 0;

        public float mantleTimeVert = .8f, mantleTimeHor = .2f;

        public float wallClimbTime = 2f, climbSpeed = 4;

        private void Update(){
            CheckIfNearWall();
        }

        protected override void VariablesSetUp(){
            CreateCheckers();
        }

        private void CreateCheckers(){
            _checkers = new List<Vector3>();

            _distanceBetweenCheckers = _playerHeight / _numOfCheckers;
            float halfHeight = _playerHeight / 2;
            Vector3 topPos = _centerOfPlayer + Vector3.up * halfHeight;

            for (int i = 0; i < _numOfCheckers; i++){
                var checkerPos = topPos + Vector3.down * _distanceBetweenCheckers * i;

                _checkers.Add(checkerPos);
            }

            _checkers.Reverse();
        }

        private void CheckIfNearWall(){
            if (!Physics.Raycast(transform.position + _checkers[0], transform.forward, _distance, ground)) return;

            // Da cambiare per ledge fini
            
            bool atLedge = false;
            int index = 999;

            for (int i = 1; i < _checkers.Count; i++){
                if (Physics.Raycast(transform.position + _checkers[i], transform.forward, _distance, ground)){
                    atLedge = false;
                    //Debug.Log("Cast " + i + " hiteth");
                    continue;
                }

                atLedge = true;
                if (i < index){
                    index = i;
                }

                //Debug.Log("Cast " + i + " missed");
            }

            if (atLedge && !isMantling){
                //print("At ledge and the free raycast is the number " + (index + 1));
                StartCoroutine(Mantle(index + 1));
            }
            else{
                if (_pm.currentState == PlayerMovement.PlayerStates.Running && !isWallClimbing){
                    StartCoroutine(WallClimb());
                }
            }
        }

        private IEnumerator WallClimb(){
            isWallClimbing = true;

            _pm.currentState = PlayerMovement.PlayerStates.WallRunning;
            
            var time = 0f;

            RaycastHit hit;

            while (time < wallClimbTime){
                if(Physics.Raycast(transform.position + _checkers[(int)_checkers.Count/2], transform.forward, out hit, ground))
                {
                    var climbDir = Vector3.Cross(hit.normal, -transform.right);
                    _pm.MovementDirection = climbDir * climbSpeed;
                    _pm.controller.Move(_pm.MovementDirection * Time.deltaTime);
                }

                time += Time.deltaTime;
                yield return null;
            }

            _pm.currentState = PlayerMovement.PlayerStates.Idle;
            isWallClimbing = false;
            yield return null;
        }

        private IEnumerator Mantle(int index){
            isMantling = true;
            
            StopCoroutine(WallClimb());
            isWallClimbing = false;
            
            _pm.ChangePlayerState(PlayerMovement.PlayerStates.Mantling);

            var start = transform.position;
            var targetPos = start + Vector3.up * _distanceBetweenCheckers * index;
            var time = 0f;
            
            while (transform.position != targetPos){
                transform.position = Vector3.Lerp(start, targetPos, time / mantleTimeVert);
                time += Time.deltaTime;
                yield return null;
            }
            start = transform.position;
            targetPos = transform.position + transform.forward * _distance * 1.5f;

            time = 0;
            
            while (transform.position != targetPos){
                transform.position = Vector3.Lerp(start, targetPos, time / mantleTimeHor);
                time += Time.deltaTime;
                yield return null;
            }
            
            _pm.ChangePlayerState(PlayerMovement.PlayerStates.Idle);
            
            isMantling = false;
            yield return null;
        }

        
        private void OnDrawGizmosSelected(){
            float distanceBetweenCheckers = _playerHeight / _numOfCheckers;
            float halfHeight = _playerHeight / 2;
            Vector3 topPos = _centerOfPlayer + Vector3.up * halfHeight;

            for (int i = 0; i < _numOfCheckers; i++){
                var checkerPos = topPos + Vector3.down * distanceBetweenCheckers * i;
                var pos = transform.position + checkerPos;
                Gizmos.DrawSphere(pos, .1f);
                Gizmos.DrawLine(pos, pos + transform.forward * _distance);
            }
        }
    }
}