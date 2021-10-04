
using UnityEngine;

namespace Advanced_Weapon_System {
	
	public enum FireType{Muanual,SemiAutomatic,Automatic}

	[CreateAssetMenu(menuName = "Data/Settings/WeaponSettings")]
	public class WeaponSettings : ScriptableObject {
		public bool showBouncingPreview = false;

		[Tooltip("BulletPerSecond")] public float fireRate;
		public FireType fireType;
		public float fireRaffic;
		public int maxBullets;

	}

}
