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



using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Advanced_Movement.Individual_Mechanics{
    public class MantleAndWallClimb : Mechanic{
        private List<Vector3> _checkers;

        private int _numOfCheckers = 4;
        private float _playerHeight = 2;
        private Vector3 _centerOfPlayer = Vector3.zero;
        private float _distance = .5f;

        private LayerMask _ground;

        private bool _isMantling = false, _isWallClimbing = false;

        private float _distanceBetweenCheckers = 0;

        private float _mantleTimeVert = .8f, _mantleTimeHor = .2f;

        private float _wallClimbTime = 2f, _climbSpeed = 4;

        private bool _canWallClimb;

        private void Update(){
            CheckIfNearWall();
        }

        protected override void VariablesSetUp(){
            _numOfCheckers = _pm.numberOfCheckers;
            _playerHeight = _pm.playerHeight;
            _centerOfPlayer = _pm.centerOfPlayer;
            _distance = _pm.checkDistance;

            _mantleTimeHor = _pm.mantleTimeHor;
            _mantleTimeVert = _pm.mantleTimeVert;

            _wallClimbTime = _pm.wallClimbTime;
            _climbSpeed = _pm.wallClimbSpeed;

            _ground = _pm.groundMask;

            _canWallClimb = _pm.canWallClimb;
            
            CreateCheckers();
        }

        protected override void CheckIfEnabled(){
            enabled = _pm.mechanicsDictionary[AdvancedMovement.Mechanics.MantleAndWallClimb.ToString()];
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
            bool atLedge = false;
            bool atWall = false;
            int index = 999;

            for (int i = 0; i < _checkers.Count - 1; i++){
                if (Physics.Raycast(transform.position + _checkers[i], transform.forward, _distance, _ground)){
                    atWall = true;
                }
                else{
                    if (!atWall || atLedge) continue;
                    atLedge = true;
                    if (i < index){
                        index = i;
                    }
                }
            }

            if (atLedge && !_isMantling && 
                (_pm.currentState == AdvancedMovement.PlayerStates.InAir || 
                _pm.currentState == AdvancedMovement.PlayerStates.WallClimbing || 
                _pm.currentState == AdvancedMovement.PlayerStates.WallRunning)){
                StopAllCoroutines();
                _isWallClimbing = false;
                StartCoroutine(Mantle(index + 1));
            }
            else{
                if (_canWallClimb && atWall && _pm.currentState == AdvancedMovement.PlayerStates.Running && !_isWallClimbing){
                    StartCoroutine(WallClimb());
                }
            }
        }

        private IEnumerator WallClimb(){
            _isWallClimbing = true;

            _pm.currentState = AdvancedMovement.PlayerStates.WallClimbing;
            
            var time = 0f;

            RaycastHit hit;

            while (time < _wallClimbTime){
                if(Physics.Raycast(transform.position + _checkers[(int)_checkers.Count/2], transform.forward, out hit, _ground))
                {
                    var climbDir = Vector3.Cross(hit.normal, -transform.right);
                    _pm.MovementDirection = climbDir * _climbSpeed;
                    _pm.controller.Move(_pm.MovementDirection * Time.deltaTime);
                }

                time += Time.deltaTime;
                yield return null;
            }
            _pm.currentState = AdvancedMovement.PlayerStates.Idle;
            _isWallClimbing = false;
            yield return null;
        }

        private IEnumerator Mantle(int index){
            _isMantling = true;
            _pm.ChangePlayerState(AdvancedMovement.PlayerStates.Mantling);

            var start = transform.position;
            var targetPos = start + Vector3.up * _distanceBetweenCheckers * index;
            var time = 0f;
            
            while (transform.position != targetPos){
                transform.position = Vector3.Lerp(start, targetPos, time / _mantleTimeVert);
                time += Time.deltaTime;
                yield return null;
            }

            //print("First");
            
            start = transform.position;
            targetPos = transform.position + transform.forward * _distance * 1.5f;

            time = 0;
            
            while (transform.position != targetPos){
                transform.position = Vector3.Lerp(start, targetPos, time / _mantleTimeHor);
                time += Time.deltaTime;
                yield return null;
            }
            
            //print("Second");
            
            _pm.ChangePlayerState(AdvancedMovement.PlayerStates.Idle);
            
            _isMantling = false;
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