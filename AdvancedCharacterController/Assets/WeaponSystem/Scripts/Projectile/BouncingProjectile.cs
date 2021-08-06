using System;
using UnityEditor;
using UnityEngine;

namespace Advanced_Weapon_System {

	public class BouncingProjectile : BehaviourProjectile {

		[SerializeField]private int bounce=1;
		
		private void Awake() {
			projectile.Hit += OnHit;
			//bouncing sbagliato
			bounce = projectile.settings.bouncingSettings.maxReflectionCount;
		}

		private void OnHit(Collision other) {
			Debug.Log("Bouncing");
			if (bounce > 0) {
				transform.forward = Vector3.Reflect(transform.forward, other.contacts[0].normal);
			}
			else {
				StartCoroutine(projectile.DestroyProjectile());
			}
		}

#if UNITY_EDITOR
		void OnDrawGizmos()
		{
			Handles.color = Color.red;
			Handles.ArrowHandleCap(0, transform.position + transform.forward * 0.25f, transform.rotation, 0.5f, EventType.Repaint);
			Gizmos.color = Color.red;
			Gizmos.DrawWireSphere(transform.position, 0.25f);

			DrawPredictedReflectionPattern(transform.position + transform.forward * 0.75f, transform.forward);
		}

		private void DrawPredictedReflectionPattern(Vector3 position, Vector3 direction)
		{
			if (bounce == 0) {
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

			Gizmos.color = Color.yellow;
			Gizmos.DrawLine(startingPosition, position);
			bounce--;
			DrawPredictedReflectionPattern(position, direction);
		}
	}
	
#endif
}