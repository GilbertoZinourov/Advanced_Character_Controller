using UnityEngine;

namespace Advanced_Weapon_System {

	[CreateAssetMenu(menuName = "Data/Settings/MovementSettings")]
	public class MovementSettings : ScriptableObject {
		public MovementSettingsData settingsData;

		public MovementSettings(MovementSettings settings) {
			settingsData = settings.settingsData;
		}
	}

}