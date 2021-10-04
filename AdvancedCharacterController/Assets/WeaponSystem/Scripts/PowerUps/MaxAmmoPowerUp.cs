

public class MaxAmmoPowerUp : TogglePowerUp {
	public int addedAmmo;

	public override void Apply() {
		weaponSystem.MainWeapon.AddMaxAmmo(addedAmmo);
	}

	public override void Revert() {
		weaponSystem.MainWeapon.AddMaxAmmo(-addedAmmo);
	}
}
