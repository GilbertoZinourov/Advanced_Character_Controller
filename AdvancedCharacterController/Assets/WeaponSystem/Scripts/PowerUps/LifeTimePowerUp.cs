

public class LifeTimePowerUp : TogglePowerUp {
    public float addedLifeTime;

    public override void Apply() {
        weaponSystem.MainWeapon.Projectile.projectileSettings.movementSettings.lifeTime += addedLifeTime;
    }

    public override void Revert() {
        weaponSystem.MainWeapon.Projectile.projectileSettings.movementSettings.lifeTime -= addedLifeTime;
    }

}
