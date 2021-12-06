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



using UnityEngine;

namespace Advanced_Movement{
    public class PlayerInputs : MonoBehaviour{
        private AdvancedMovement _movement;
        private Vector2 _inputWASD;
        private Vector2 _inputMouse;

        private void Awake(){
            _movement = GetComponent<AdvancedMovement>();
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        private void Update(){
            EightDirMovement();
            Jump();
        }

        private void Jump(){
            if (Input.GetKeyDown(KeyCode.Space)){
                _movement.Jump();
            }
        }

        private void EightDirMovement(){
            GetWASDInput();

            _movement.Move(_inputWASD);


            if (Input.GetKeyDown(KeyCode.LeftShift)){
                _movement.Running();
            }
            
            if (Input.GetKeyDown(KeyCode.LeftControl)){
                _movement.Crouching();
            }

            GetMouseInput();
            LookAround();
        }

        private void GetWASDInput(){
            _inputWASD = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        }

        private void LookAround(){
            GetMouseInput();
            _movement.Rotate(_inputMouse);
        }

        private void GetMouseInput(){
            _inputMouse = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        }
    }
}