using System;
using UnityEngine;

namespace Advanced_Weapon_System {

	public abstract class MovementProjectile : ProjectileComponent {
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
			set {
				transform.forward = value;
				oldForward = value;
			} 
		}
		
		public Vector3 oldForward;

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