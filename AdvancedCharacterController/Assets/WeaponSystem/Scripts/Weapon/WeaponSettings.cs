using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Advanced_Weapon_System {
	
	public enum FireType{Muanual,SemiAutomatic,Automatic}

	[CreateAssetMenu(menuName = "Data/Settings/WeaponSettings")]
	public class WeaponSettings : ScriptableObject {
		public bool showBouncingPreview = false;
		public bool showExplosivePreview = false;

		[Tooltip("BulletPerSecond")] public float fireRate;
		public FireType fireType;
		public float fireRaffic;
		public float maxBullets;

	}

}
