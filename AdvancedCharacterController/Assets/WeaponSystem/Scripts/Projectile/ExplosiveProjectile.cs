using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Advanced_Weapon_System {

	public class ExplosiveProjectile : BehaviourProjectile {
		
		private readonly List<GameObject> explosions=new List<GameObject>();

		public override void Init() {
			projectile.Hit += OnHit;
		}

		private void OnHit(Collision obj) {
			StartCoroutine(DelayedExplosion(projectile.destroyOnFirstHit));
		}

		private IEnumerator DelayedExplosion(bool shouldDestroyProjectile=false) {
			float timer = 0;
			while (timer < projectile.projectileSettings.explosiveSettings.explosionDelay) {
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
			
			GameObject explosionObject = Instantiate(projectile.projectileSettings.explosiveSettings.explosivePrefab,transform.position,transform.rotation);
			explosions.Add(explosionObject);
			explosionObject.GetComponent<Explosion>()?.SetExplosion(projectile.projectileSettings.explosiveSettings.smallRadius,projectile.projectileSettings.explosiveSettings.bigRadius);
			
			float timer = 0;
			while (timer < projectile.projectileSettings.explosiveSettings.explosionDuration) {
				timer += Time.deltaTime;
				
				//sphereCast for damage
				yield return null;
			}
			
			explosions.Remove(explosionObject);
			Destroy(explosionObject);
		}

		private void OnDestroy() {
			foreach (GameObject explosion in explosions) {
				Destroy(explosion);
			}
		}
	}
	

}