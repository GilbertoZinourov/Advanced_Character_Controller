using UnityEngine;

namespace Advanced_Weapon_System {

	[CreateAssetMenu(menuName = "Data/Settings/BouncingSettings")]
	public class BouncingSettings : ScriptableObject {
		public int maxReflectionCount = 5;
		public float maxStepDistance = 200;
	}

}