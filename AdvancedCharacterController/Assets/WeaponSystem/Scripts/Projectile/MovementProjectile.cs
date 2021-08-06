using System;
using UnityEngine;

namespace Advanced_Weapon_System {

	public class MovementProjectile : ProjectileComponent {
		protected Rigidbody rigidbody=default;
		
		protected float speed;
		public float Speed {
			get {
				if (isStopped) {
					return 0;
				}
				else {
					return GetSpeed();
				}
			}
			set {
				speed = value;
			}
		}

		public Vector3 Dir {
			get {
				if (isStopped) {
					return Vector3.zero;
				}
				else {
					return transform.forward;
				}
			}
			set => transform.forward = value;
		}

		protected bool isStopped;
		
		protected float GetSpeed() {
			return projectile.settings.movementSettings.graphic.Evaluate(projectile.Timer);
		}

		public void StopProjectile() {
			isStopped = true;
			Debug.Log("Projectile stopped");
		}
		
	}

}