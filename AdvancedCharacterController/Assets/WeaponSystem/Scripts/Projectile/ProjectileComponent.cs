using Unity.Collections;
using UnityEngine;

namespace Advanced_Weapon_System {

	public class ProjectileComponent : MonoBehaviour {

		[SerializeField, HideInInspector] protected Projectile projectile;

		public void InitializeComponent(Projectile projectile) {
			this.projectile = projectile;
		}
	}

}