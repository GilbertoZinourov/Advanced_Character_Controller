using UnityEngine;

namespace Advanced_Weapon_System {

	[CreateAssetMenu(menuName = "Data/Settings/MovementSettings")]
	public class MovementSettings : ScriptableObject {
		public AnimationCurve graphic;
		public float lifeTime;
	}

}