using System;
using System.Collections;
using UnityEngine;

namespace Advanced_Weapon_System {

	public class ExplosiveProjectile : BehaviourProjectile {

		private void Awake() {
			projectile.Hit += OnHit;
		}

		private void OnHit(Collision other) {
			StartCoroutine(DelayedExplosion());
		}

		private IEnumerator DelayedExplosion() {
			float timer = 0;
			while (timer < projectile.settings.explosiveSettings.explosionDelay) {
				timer += Time.deltaTime;
				yield return null;
			}
			timer = 0;
			Debug.Log("Explode");
			while (timer < projectile.settings.explosiveSettings.explosionDuration) {
				//sphereCast
				//Lascia un oggetto dietro fi 
				timer += Time.deltaTime;
				yield return null;
			}
			Debug.Log("Explosion ended");
		}
		
	}

}