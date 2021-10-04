
using UnityEngine;

public abstract class TogglePowerUp : PowerUp {
    
    public override void Interact() {
        weaponSystem=FindObjectOfType<WeaponSystem>();
        weaponSystem.MainWeapon.AddPowerUp(this);
    }
    public abstract void Revert();
}

public abstract class PowerUp : MonoBehaviour,IInteractable {
    protected WeaponSystem weaponSystem;
    public virtual void Interact() {
        weaponSystem=FindObjectOfType<WeaponSystem>();
        Apply();
    }

    public abstract void Apply();
}
