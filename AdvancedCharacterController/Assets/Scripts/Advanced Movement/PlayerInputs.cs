using UnityEngine;

namespace Advanced_Movement{
    public class PlayerInputs : MonoBehaviour{
        private PlayerMovement _movement;
        private Vector2 _inputWASD;
        private Vector2 _inputMouse;

        private void Awake(){
            _movement = GetComponent<PlayerMovement>();
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