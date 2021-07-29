using System;
using UnityEngine;

namespace Advanced_Weapon_System {

	public class MovementProjectile : ProjectileComponent {
		protected Rigidbody rigidbody=default;
		protected float speed;
		public float Speed {
			get {
				speed = GetSpeed();
				return speed;
			}
		}

		protected Vector3 dir;
		public Vector3 Dir {
			get => dir;
			set => dir = value;
		}

		public virtual void Update() {
			speed=GetSpeed();
		}

		protected float GetSpeed() {
			return settings.Graphic.Evaluate(projectile.Timer);
		}

	}

}