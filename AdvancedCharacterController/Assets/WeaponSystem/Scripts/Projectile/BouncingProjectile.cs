﻿using System;
using UnityEditor;
using UnityEngine;

namespace Advanced_Weapon_System {

	public class BouncingProjectile : BehaviourProjectile {
		
		private void OnCollisionEnter(Collision other) {
			Debug.Log("Bouncing");
			transform.forward = Vector3.Reflect(transform.forward, other.contacts[0].normal);
		}

		// private void Update() {
		// 	RaycastHit hit;
		// 	if (Physics.Raycast(transform.position, transform.forward,out hit, 0.1f)) {
		// 		Debug.Log("Bouncing");
		// 		transform.forward = Vector3.Reflect(transform.forward, hit.normal);
		// 	}
		// }

		public int maxReflectionCount = 5;
		public float maxStepDistance = 200;

		void OnDrawGizmos()
		{
			Handles.color = Color.red;
			Handles.ArrowHandleCap(0, this.transform.position + this.transform.forward * 0.25f, this.transform.rotation, 0.5f, EventType.Repaint);
			Gizmos.color = Color.red;
			Gizmos.DrawWireSphere(this.transform.position, 0.25f);

			DrawPredictedReflectionPattern(this.transform.position + this.transform.forward * 0.75f, this.transform.forward, maxReflectionCount);
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