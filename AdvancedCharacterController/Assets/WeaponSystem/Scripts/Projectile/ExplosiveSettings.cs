using UnityEngine;

namespace Advanced_Weapon_System {

	[CreateAssetMenu(menuName = "Data/Settings/ExplosiveSettings")]
	public class ExplosiveSettings : ScriptableObject {
		public float smallRadius;
		public float smallRadiusDamage;
		public float bigRadius;
		public float bigRadiusDamage;

		public GameObject explosivePrefab;
		
		public float explosionDelay;
		public float explosionDuration;
		public bool explodeOnHit;
		public bool movementExplosion;
	}

}