using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Advanced_Movement.Individual_Mechanics{
    public class MantleAndWallClimb : Mechanic{
        public List<Vector3> _checkers;

        public int _numOfCheckers = 4;
        public float _playerHeight = 2;
        public Vector3 _centerOfPlayer = Vector3.zero;
        public float _distance = .5f;

        public LayerMask ground;

        public bool isMantling = false;

        private float _distanceBetweenCheckers = 0;

        public float mantleTimeVert = .8f, mantleTimeHor = .2f;

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
        }

        private IEnumerator Mantle(int index){
            isMantling = true;
            _pm.ChangePlayerState(PlayerMovement.PlayerStates.Mantling);

            var start = transform.position;
            var targetPos = start + Vector3.up * _distanceBetweenCheckers * index;
            var time = 0f;
            
            while (transform.position != targetPos){
                transform.position = Vector3.Lerp(start, targetPos, time / mantleTimeVert);
                time += Time.deltaTime;
                yield return null;
            }
            print("Done first");
            start = transform.position;
            targetPos = transform.position + transform.forward * _distance * 1.5f;

            time = 0;
            
            while (transform.position != targetPos){
                transform.position = Vector3.Lerp(start, targetPos, time / mantleTimeHor);
                time += Time.deltaTime;
                yield return null;
            }
            print("Done all");
            
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