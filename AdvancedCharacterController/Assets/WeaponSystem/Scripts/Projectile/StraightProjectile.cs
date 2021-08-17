using System;
using UnityEngine.PlayerLoop;

namespace Advanced_Weapon_System {
	
	public class StraightProjectile : MovementProjectile {

		public override void Init() {
			
		}
		private void FixedUpdate() {
			Move();
		}

		private void Move() {
			Dir = transform.forward;
			transform.position += Dir * Speed;
		}

	}

}