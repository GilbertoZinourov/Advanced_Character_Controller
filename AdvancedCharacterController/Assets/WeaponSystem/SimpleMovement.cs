
using UnityEngine;

namespace Advanced_Weapon_System {

    public class SimpleMovement : MonoBehaviour {
        public Vector3 dir;
        public float speed=0.03f;

        public void Update() {
            dir=Vector3.zero;
            if (Input.GetKey(KeyCode.W)) {
                dir+=transform.forward;
            }
            if (Input.GetKey(KeyCode.S)) {
                dir+=-transform.forward;
            }
            if (Input.GetKey(KeyCode.A)) {
                dir+=-transform.right;
            }
            if (Input.GetKey(KeyCode.D)) {
                dir+=transform.right;
            }
            if (Input.GetKey(KeyCode.E)) {
                dir+=transform.up;
            }
            if (Input.GetKey(KeyCode.Q)) {
                dir+=-transform.up;
            }
            dir.Normalize();
        }

        private void FixedUpdate() {
            transform.position += dir * speed;
            
        }
    }

    
}