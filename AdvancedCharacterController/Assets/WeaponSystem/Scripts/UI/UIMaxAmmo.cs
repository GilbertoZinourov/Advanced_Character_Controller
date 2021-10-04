
using Advanced_Weapon_System;
using UnityEngine;
using UnityEngine.UI;

public class UIMaxAmmo : MonoBehaviour {
    public Slider slider;
    private Weapon weapon;

    private void Start() {
        weapon = GetComponent<Weapon>();
        weapon.BulletChange += OnChangeCurrenAmmo;
        weapon.Initialized += Init;
        Init();
    }

    private void Init() {
        slider.minValue = 0;
        slider.maxValue = weapon.WeaponSettings.maxBullets;
        slider.value = weapon.WeaponSettings.maxBullets;
    }

    private void OnChangeCurrenAmmo() {
        slider.value = weapon.CurrentBullet;
        slider.maxValue = weapon.WeaponSettings.maxBullets;
    }
}
