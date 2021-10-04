using UnityEngine;

namespace Advanced_Weapon_System {

	public class GravityProjectile : MovementProjectile {
		
		public override void Init() {
			Dir = transform.forward;
		}
		
		private void FixedUpdate() {
			Move();
		}

		private void Move() {
			var gravityVector=projectile.rb.velocity.normalized;
			var speedVector=oldForward * Speed;
			var newVector = gravityVector + speedVector.normalized;
			transform.forward = newVector.normalized;
			transform.position += speedVector;
		}
	}

}