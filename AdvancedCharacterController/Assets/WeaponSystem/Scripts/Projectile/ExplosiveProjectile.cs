using System;
using System.Collections;
using UnityEngine;

namespace Advanced_Weapon_System {

	public class ExplosiveProjectile : BehaviourProjectile {

		public override void Init() {
			if (projectile.settings.explosiveSettings.explodeOnHit) {
				projectile.Hit += OnHit;
			}
			else {
				StartCoroutine(DelayedExplosion(true));
			}
		}

		private void OnHit(Collision other) {
			StartCoroutine(DelayedExplosion());
		}
		

		private IEnumerator DelayedExplosion(bool shouldDestroyProjectile=false) {
			float timer = 0;
			while (timer < projectile.settings.explosiveSettings.explosionDelay) {
				timer += Time.deltaTime;
				yield return null;
			}
			yield return Explosion(shouldDestroyProjectile);
			if (shouldDestroyProjectile) {
				StartCoroutine(projectile.DestroyProjectile(true));
			}
		}

		private IEnumerator Explosion(bool shouldDestroyProjectile=false) {
			if (shouldDestroyProjectile) {
				projectile.StopProjectile();
			}
			GameObject explosionObject;
			float timer = 0;
			Debug.Log("Explode");
			if (projectile.settings.explosiveSettings.movementExplosion) {
				//explosionObject = Instantiate(projectile.settings.explosiveSettings.explosivePrefab,transform.position,transform.rotation,transform);
				explosionObject = Instantiate(projectile.settings.explosiveSettings.explosivePrefab,transform.position,transform.rotation);
			}
			else {
				explosionObject = Instantiate(projectile.settings.explosiveSettings.explosivePrefab,transform.position,transform.rotation);
			}
			
			explosionObject.GetComponent<Explosion>()?.SetExplosion(projectile.settings.explosiveSettings.smallRadius,projectile.settings.explosiveSettings.bigRadius);
			while (timer < projectile.settings.explosiveSettings.explosionDuration) {
				//sphereCast
				timer += Time.deltaTime;
				yield return null;
			}
			Destroy(explosionObject);
			Debug.Log("Explosion ended");
		}
		
	}

}