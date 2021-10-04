

public class DamagePowerUp : TogglePowerUp {
	public float addedDamage;

	public override void Apply() {
		weaponSystem.MainWeapon.Projectile.projectileSettings.bulletDamage += addedDamage;
	}

	public override void Revert() {
		weaponSystem.MainWeapon.Projectile.projectileSettings.bulletDamage -= addedDamage;
	}
}
