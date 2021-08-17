using System;
using Unity.Collections;
using UnityEngine;

namespace Advanced_Weapon_System {

	public abstract class ProjectileComponent : MonoBehaviour {

		[SerializeField, HideInInspector] protected Projectile projectile;

		private void Start() {
			Init();
		}

		public void InitializeComponent(Projectile projectile) {
			this.projectile = projectile;
		}

		public abstract void Init();
	}

}