using UnityEngine;

namespace Advanced_Weapon_System {

	public class NormalProjectile: BehaviourProjectile {
		
		private void OnCollisionEnter(Collision other) {
			Debug.Log("Hit");
			Destroy(this.gameObject);
		}
	}

}