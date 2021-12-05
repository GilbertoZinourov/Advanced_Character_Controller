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