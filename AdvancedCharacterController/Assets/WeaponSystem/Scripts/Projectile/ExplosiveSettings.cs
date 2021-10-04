using UnityEngine;

namespace Advanced_Weapon_System {

	[CreateAssetMenu(menuName = "Data/Settings/ExplosiveSettings")]
	public class ExplosiveSettings : ScriptableObject {
		public ExplosiveSettingsData settingsData;

		public ExplosiveSettings(ExplosiveSettings settings) {
			settingsData = settings.settingsData;
		}
	}

}