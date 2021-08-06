using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Playables;
using UnityEngine;



namespace Advanced_Weapon_System {


	public class Weapon : MonoBehaviour {
		public GameObject projectilePrefab;
		public GameObject projectileInstance;
		public Transform firePoint;

		public bool showBouncingPreview = false;
		public bool showExplosivePreview = false;
		public Projectile projectile;
		
		private void Start() {
			//projectileInstance=(GameObject)PrefabUtility.InstantiatePrefab(projectilePrefab, transform);
			projectileInstance=Instantiate(projectilePrefab, firePoint.transform.position, transform.rotation,transform);
			projectile = projectileInstance.GetComponent<Projectile>();
			projectile.AddBehaviours();
		}

		private void Update() {
			if (Input.GetButtonDown("Fire1")) {
				var obj=Instantiate(projectileInstance, firePoint.transform.position, transform.rotation);
				obj.SetActive(true);
			}
		}

		void OnDrawGizmos()
		{
			if (showBouncingPreview) {
				Handles.color = Color.red;
				Handles.ArrowHandleCap(0, transform.position + transform.forward * 0.25f, transform.rotation, 0.25f, EventType.Repaint);

				DrawPredictedReflectionPattern(transform.position + transform.forward * 0.5f, transform.forward, projectile.settings.bouncingSettings.maxReflectionCount);
			}else if (showExplosivePreview) {
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
			if (Physics.Raycast(ray, out hit, projectile.settings.bouncingSettings.maxStepDistance))
			{
				direction = Vector3.Reflect(direction, hit.normal);
				position = hit.point;
			}
			else
			{
				position += direction * projectile.settings.bouncingSettings.maxStepDistance;
			}
			
			if (showExplosivePreview) {
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
				Gizmos.DrawWireSphere(position,projectile.settings.explosiveSettings.smallRadius);
				Gizmos.color = Color.yellow;
				Gizmos.DrawWireSphere(position,projectile.settings.explosiveSettings.bigRadius);
				Gizmos.DrawLine(startingPosition, position);
			}
			else {
				Gizmos.color = Color.yellow;
				Gizmos.DrawLine(position, direction*1000);
			}
		}
	}

}
