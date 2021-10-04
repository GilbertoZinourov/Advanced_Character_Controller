
using Advanced_Weapon_System;

public class ProjectilePowerUp : PowerUp {
	public ProjectileSettings projectileSettings;

	public override void Apply() {
		weaponSystem.MainWeapon.ProjectileSettings = projectileSettings;
	}
}
