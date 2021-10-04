

public class RechargePowerUp : PowerUp
{

	public override void Apply() {
		weaponSystem.MainWeapon.Recharge();
	}
	
}
