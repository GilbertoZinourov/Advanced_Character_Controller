using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Advanced_Weapon_System {

	public class MouseLook : MonoBehaviour {
		Vector2 rotation = Vector2.zero;
		public float speed = 3;

		private void Awake() {
			Cursor.lockState = CursorLockMode.Locked;
		}

		void Update() {
			rotation.y += Input.GetAxis("Mouse X");
			rotation.x += -Input.GetAxis("Mouse Y");
			transform.eulerAngles = (Vector2)rotation * speed;
		}
	}

}
