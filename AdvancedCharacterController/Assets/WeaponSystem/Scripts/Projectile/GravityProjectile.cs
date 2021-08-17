using UnityEngine;

namespace Advanced_Weapon_System {

	public class GravityProjectile : MovementProjectile {

		
		public override void Init() {
			Dir = transform.forward;//projectile.rb.AddForce(0,0,projectile.settings.movementSettings.graphic.Evaluate(0));
		}
		
		private void FixedUpdate() {
			Move();
		}

		private void Move() {
			//transform.forward = projectile.rb.velocity;
			var gravityVector=projectile.rb.velocity.normalized;
			var speedVector=oldForward * Speed;
			var newVector = gravityVector + speedVector.normalized;
			transform.forward = newVector.normalized;
			// var newVelocity = new Vector3( oldForward.x* Speed, gravitySpeed, oldForward.z* Speed);
			// transform.LookAt(transform.position+newVelocity.normalized);
			transform.position += speedVector;
		}
	}

}