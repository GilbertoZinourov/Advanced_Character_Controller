
using UnityEditor;
using UnityEngine;

namespace Advanced_Weapon_System {

	public class BouncingProjectile : BehaviourProjectile {

		[SerializeField]private int maxBounce=0;
		[SerializeField]private int currentBounce=1;

		public override void Init() {
			projectile.Hit += OnHit;
			maxBounce = projectile.projectileSettings.bouncingSettings.maxReflectionCount;
			currentBounce = maxBounce;
		}

		private void OnHit(Collision other) {
			if (currentBounce > 0) {
				if (projectile.collider.material != projectile.projectileSettings.gravityBouncingMaterial) {
					projectile.movementComponent.Dir = Vector3.Reflect(transform.forward, other.contacts[0].normal);
				}
				currentBounce--;
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
		
			DrawPredictedReflectionPattern(transform.position + transform.forward * 0.75f, transform.forward,maxBounce);
		}
		
		private void DrawPredictedReflectionPattern(Vector3 position, Vector3 direction,int bounce)
		{
			if (bounce == 0) {
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
		
			Gizmos.color = Color.yellow;
			Gizmos.DrawLine(startingPosition, position);
			
			DrawPredictedReflectionPattern(position, direction,bounce-1);
		}
#endif
	}
	
}