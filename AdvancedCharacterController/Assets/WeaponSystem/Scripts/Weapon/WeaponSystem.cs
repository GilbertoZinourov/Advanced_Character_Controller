using System;
using System.Collections;
using System.Collections.Generic;
using Advanced_Weapon_System;
using UnityEngine;

public class WeaponSystem : MonoBehaviour {
	public List<Weapon> equippedWeapons;
	private List<IInteractable> interactables=new List<IInteractable>();
	
	public Weapon MainWeapon {
		get {
			if (equippedWeapons.Count >= 1) {
				return equippedWeapons[0];
			}
			return null;
		}
	}

	public void InitWeapons() {
		foreach (Weapon equippedWeapon in equippedWeapons) {
			equippedWeapon.Init();
		}
	}
	
	private void Start() {
		InitWeapons();
	}

	private void Update() {
		if (Input.GetKeyDown(KeyCode.T)) {
			MainWeapon.ChangeWeaponType();
		}
		if (Input.GetButton("Fire1")) {
			MainWeapon.FireWithWeapon();
		}

		if (Input.GetKeyDown(KeyCode.UpArrow)) {
			ScrollWeapons(true);
		}
		
		if (Input.GetKeyDown(KeyCode.DownArrow)) {
			ScrollWeapons(false);
		}

		if (Input.GetKeyDown(KeyCode.F)) {
			if (interactables.Count > 0) {
				interactables?[0].Interact();
			}
		}

		if (Input.GetButtonUp("Fire1")) {
			MainWeapon.ResetRafficCount();
		}
	}

	private void ChangeWeapon() {
		MainWeapon.gameObject.SetActive(true);
		for (int i = 0; i < equippedWeapons.Count; i++) {
			if (i != 0) {
				equippedWeapons[i].gameObject.SetActive(false);
			}
		}
	}

	private void ScrollWeapons(bool forward) {
		if (forward) {
			equippedWeapons.Add(equippedWeapons[0]);
			equippedWeapons.RemoveAt(0);
		}
		else {
			equippedWeapons.Insert(0,equippedWeapons[equippedWeapons.Count-1]);
			equippedWeapons.RemoveAt(equippedWeapons.Count-1);
		}
		ChangeWeapon();
	}

	private void OnTriggerEnter(Collider other) {
		if (other.GetComponent<IInteractable>() != null) {
			interactables.Add(other.GetComponent<IInteractable>());
		}
	}

	private void OnTriggerExit(Collider other) {
		if (other.GetComponent<IInteractable>() != null) {
			interactables.Remove(other.GetComponent<IInteractable>());
		}
	}
}
