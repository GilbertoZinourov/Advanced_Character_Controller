

public class FireRatePowerUp : TogglePowerUp {
	public float addedFireRate;

	public override void Apply() {
		weaponSystem.MainWeapon.AddFireRate(addedFireRate);
	}

	public override void Revert() {
		weaponSystem.MainWeapon.AddFireRate(-addedFireRate);
	}
}
