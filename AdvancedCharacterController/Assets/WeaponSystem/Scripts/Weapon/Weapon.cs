using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Playables;
using UnityEngine;

namespace Advanced_Weapon_System {
	
	public class Weapon : MonoBehaviour {
		public WeaponSettings weaponSettings;
		public GameObject projectilePrefab;
		[HideInInspector] public GameObject projectileInstance;
		public Transform firePoint;
		[HideInInspector] public Projectile projectile;

		private float timer = 0;
		private bool canFire = false;
		private int fireRafficCount = 0;
		
		private void Start() {
			//projectileInstance=(GameObject)PrefabUtility.InstantiatePrefab(projectilePrefab, transform);
			projectileInstance=Instantiate(projectilePrefab, firePoint.transform.position, transform.rotation,transform);
			projectile = projectileInstance.GetComponent<Projectile>();
			projectile.AddBehaviours();
			timer = 0;
			canFire = false;
			fireRafficCount = 0;
		}

		private void Update() {
			timer += Time.deltaTime;
			if (timer > 1 / weaponSettings.fireRate) {
				canFire = true;
			}
			if (canFire) {
				switch (weaponSettings.fireType) {
					case FireType.Muanual:
						if (Input.GetButtonDown("Fire1")) {
							Fire();
						}
						break;
					case FireType.SemiAutomatic:
						if (fireRafficCount < weaponSettings.fireRaffic) {
							if (Input.GetButton("Fire1")) {
								Fire();
								fireRafficCount++;
							}
						}
						break;
					case FireType.Automatic:
						if (Input.GetButton("Fire1")) {
							Fire();
						}
						break;
				}
			}
			if (Input.GetButtonUp("Fire1")) {
				fireRafficCount = 0;
			}
		}

		private void Fire() {
			var obj=Instantiate(projectileInstance, firePoint.transform.position, transform.rotation);
			obj.SetActive(true);
			canFire = false;
			timer = 0;
			
		}
		
		void OnDrawGizmos()
		{
			if (weaponSettings.showBouncingPreview) {
				Handles.color = Color.red;
				Handles.ArrowHandleCap(0, transform.position + transform.forward * 0.25f, transform.rotation, 0.25f, EventType.Repaint);

				DrawPredictedReflectionPattern(transform.position + transform.forward * 0.5f, transform.forward, projectile.projectileSettings.bouncingSettings.maxReflectionCount);
			}else if (weaponSettings.showExplosivePreview) {
				DrawPredictedExplosivePattern(transform.position + transform.forward * 0.5f,transform.forward);
			}
		}

		private void DrawPredictedReflectionPattern(Vector3 position, Vector3 direction, int reflectionsRemaining)
		{
			if (reflectionsRemaining == 0) {
				return;
			}

			Vector3 startingPosition = position;

			Ray ray = new Ray(position, direction);
			RaycastHit hit;
			if (Physics.Raycast(ray, out hit, projectile.projectileSettings.bouncingSettings.maxStepDistance))
			{
				direction = Vector3.Reflect(direction, hit.normal);
				position = hit.point;
			}
			else
			{
				position += direction * projectile.projectileSettings.bouncingSettings.maxStepDistance;
			}
			
			if (weaponSettings.showExplosivePreview) {
				DrawPredictedExplosivePattern(position, direction);
			}
			Gizmos.color = Color.green;
			Gizmos.DrawLine(startingPosition, position);
			DrawPredictedReflectionPattern(position, direction, reflectionsRemaining - 1);
		}

		private void DrawPredictedExplosivePattern(Vector3 position, Vector3 direction) {
			Vector3 startingPosition = position;

			Ray ray = new Ray(position, direction);
			RaycastHit hit;
			if (Physics.Raycast(ray, out hit))
			{
				position = hit.point;
				Gizmos.color = Color.red;
				Gizmos.DrawWireSphere(position,projectile.projectileSettings.explosiveSettings.smallRadius);
				Gizmos.color = Color.yellow;
				Gizmos.DrawWireSphere(position,projectile.projectileSettings.explosiveSettings.bigRadius);
				Gizmos.DrawLine(startingPosition, position);
			}
			else {
				Gizmos.color = Color.yellow;
				Gizmos.DrawLine(position, direction*1000);
			}
		}
	}

}
