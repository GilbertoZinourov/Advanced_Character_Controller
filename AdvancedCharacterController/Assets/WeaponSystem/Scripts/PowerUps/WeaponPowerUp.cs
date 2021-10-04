
using Advanced_Weapon_System;

public class WeaponPowerUp : PowerUp {
	public WeaponSettings weaponSettings;

	public override void Apply() {
		weaponSystem.MainWeapon.WeaponSettings = weaponSettings;
	}
}
