using UnityEngine;

namespace Advanced_Weapon_System {
	public enum BehaviourType{Normal,Bouncing,Explosive}
	public enum MovementType{Straight,Gravity}

	[CreateAssetMenu(menuName = "Data/Settings/Settings")]
	public class Settings : ScriptableObject {
		public ProjectileSettings projectileSettings;
		public WeaponSettings weaponSettings;
	}

}