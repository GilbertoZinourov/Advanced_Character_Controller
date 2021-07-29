using Unity.Collections;
using UnityEngine;

namespace Advanced_Weapon_System {

	public class ProjectileComponent : MonoBehaviour {

		[SerializeField, HideInInspector] protected Projectile projectile;
		[SerializeField, HideInInspector] protected Settings settings;

		public void InitializeComponent(Projectile projectile, Settings settings) {
			this.projectile = projectile;
			this.settings = settings;
		}
	}

}