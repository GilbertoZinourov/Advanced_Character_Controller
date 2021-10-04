using System;
using System.Collections;
using System.Collections.Generic;
using Advanced_Weapon_System;
using TMPro;
using UnityEngine;

public class UIWeaponType : MonoBehaviour
{
    public TextMeshProUGUI textMeshPro;
    private Weapon weapon;

    private void Start() {
        weapon = GetComponent<Weapon>();
        weapon.TypeChanged += OnTypeChange;
        weapon.Initialized += Init;
        Init();
    }

    private void Init() {
        textMeshPro.text = GetTypeString(weapon.WeaponSettings.fireType);
    }

    private void OnTypeChange() {
        textMeshPro.text = GetTypeString(weapon.WeaponSettings.fireType);
    }

    private string GetTypeString(FireType type) {
        switch (type) {
            case FireType.Muanual:
                return "M";
            case FireType.SemiAutomatic:
                return "S/A";
            case FireType.Automatic:
                return "A";
        }
        return "";
    }
}
