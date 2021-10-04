
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIPowerUpItem : MonoBehaviour {
    public TextMeshProUGUI name;
    public Toggle equipped;
    public TogglePowerUp powerUp;

    private void Awake() {
        equipped.onValueChanged.AddListener(EquipPowerUp);
    }

    public void Init(TogglePowerUp powerUp) {
        name.text = powerUp.name;
        this.powerUp = powerUp;
        equipped.isOn = true;
        EquipPowerUp(true);
    }

    private void EquipPowerUp(bool equip) {
        if (equipped.isOn) {
            powerUp.Apply();
        }
        else {
            powerUp.Revert();
        }
    }
}
