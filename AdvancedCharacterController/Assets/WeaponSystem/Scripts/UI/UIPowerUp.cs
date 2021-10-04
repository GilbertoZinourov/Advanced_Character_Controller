using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPowerUp : MonoBehaviour {
    public GameObject container;
    public UIPowerUpItem prefab;

    public List<UIPowerUpItem> instantiatedObject;
    
    public void CreatePowerUp(TogglePowerUp powerUp) {
        var obj = Instantiate(prefab, container.transform);
        obj.Init(powerUp);
    }
}
