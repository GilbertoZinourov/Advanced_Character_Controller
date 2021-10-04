using System;
using System.Collections;
using System.Collections.Generic;
using Advanced_Weapon_System;
using UnityEngine;
using UnityEngine.UI;

public class UIWeaponCoolDown : MonoBehaviour
{
	public Slider slider;
	private Weapon weapon;

	private void Start() {
		weapon = GetComponent<Weapon>();
		weapon.Initialized += Init;
		weapon.FireRateChanged += Init;
		Init();
	}

	private void Init() {
		slider.minValue = 0;
		slider.maxValue = 1/weapon.WeaponSettings.fireRate;
		slider.value = 1/weapon.WeaponSettings.fireRate;
	}

	private void Update() {
		slider.value = weapon.Timer;
	}
}
