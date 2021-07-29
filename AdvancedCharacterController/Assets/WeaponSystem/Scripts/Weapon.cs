using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;



namespace Advanced_Weapon_System {
	
	public enum ProjectileBehavior{Normal,Bouncing,Explosive}
	public enum ProjectileMovement{Straight,Aiming,GravityAffected}
	public enum ProjectileSpeed{Normal,Graphical}
	
	
	public class Weapon : MonoBehaviour {
		public GameObject projectilePrefab;
		public Transform firePoint;

		public bool showBouncingPreview = false;
		
		private Projectile projectile;
		
		private void Start() {
			projectilePrefab=Instantiate(projectilePrefab, firePoint.transform.position, transform.rotation,transform);
			projectile = projectilePrefab.GetComponent<Projectile>();
			projectile.AddBehaviours();
		}

		private void Update() {
			if (Input.GetButtonDown("Fire1")) {
				var obj=Instantiate(projectilePrefab, firePoint.transform.position, transform.rotation);
				obj.SetActive(true);
			}
		}
		
		public int maxReflectionCount = 5;
		public float maxStepDistance = 200;

		void OnDrawGizmos()
		{
			if (showBouncingPreview) {
				Handles.color = Color.red;
				Handles.ArrowHandleCap(0, transform.position + transform.forward * 0.25f, transform.rotation, 0.5f, EventType.Repaint);
				Gizmos.color = Color.red;
				Gizmos.DrawWireSphere(transform.position, 0.25f);

				DrawPredictedReflectionPattern(transform.position + transform.forward * 0.75f, transform.forward, maxReflectionCount);
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
			if (Physics.Raycast(ray, out hit, maxStepDistance))
			{
				direction = Vector3.Reflect(direction, hit.normal);
				position = hit.point;
			}
			else
			{
				position += direction * maxStepDistance;
			}

			Gizmos.color = Color.yellow;
			Gizmos.DrawLine(startingPosition, position);

			DrawPredictedReflectionPattern(position, direction, reflectionsRemaining - 1);
		}
	}

}
