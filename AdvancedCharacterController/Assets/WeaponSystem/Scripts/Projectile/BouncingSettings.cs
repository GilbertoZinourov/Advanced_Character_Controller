using UnityEngine;

namespace Advanced_Weapon_System {

	[CreateAssetMenu(menuName = "Data/Settings/BouncingSettings")]
	public class BouncingSettings : ScriptableObject {
		public BouncingSettingsData settingsData;

		public BouncingSettings(BouncingSettings settings) {
			settingsData = settings.settingsData;
		}
	}

}