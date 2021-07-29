using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace Advanced_Weapon_System {

	public class Projectile : MonoBehaviour {
		public ProjectileSettings projectileSettings;
		public Settings settings;
		private Rigidbody rigidbody;

		private MovementProjectile movementComponent;
		private List<BehaviourProjectile> behaviourComponents=new List<BehaviourProjectile>();
		
		private float timer=0;
		public float Timer => timer;

		private void OnEnable() {
			timer = 0;
			settings = projectileSettings.settings;
		}

		private void Update() {
			timer += Time.deltaTime;
			//Da scommentare questa riga quando si farà un input di powerup
			settings = projectileSettings.settings;
			if (timer > settings.LifeTime) {
				Destroy(gameObject);
			}
		}

		private void OnBecameInvisible() {
			Destroy(gameObject);
		}
		
		public void AddBehaviours() {
			movementComponent = null;
			behaviourComponents.Clear();
			AddMovementBehaviour();
			AddProjectileBehaviours();
		}

		private void AddMovementBehaviour() {
			switch(settings.MovementType)
			{
				case MovementType.Straight:
					InitializeMovementComponent<StraightProjectile>();
					break;
				case MovementType.Aiming:
					InitializeMovementComponent<AimingProjectile>();
					break;
				case MovementType.Gravity:
					InitializeMovementComponent<GravityProjectile>();
					break;
				default:
					Debug.LogWarning("MovementType not found");
					break;
			}
		}

		private void InitializeMovementComponent<T>() where T : MovementProjectile {
			movementComponent = gameObject.AddComponent<T>();
			movementComponent.InitializeComponent(this,settings);
		}

		private void AddProjectileBehaviours() {
			foreach (BeahviourType projectileSettingsBeahviourType in settings.BeahviourTypes) {
				switch (projectileSettingsBeahviourType) {
					case BeahviourType.Normal:
						InitializeBehaviourComponent<NormalProjectile>();
						break;
					case BeahviourType.Bouncing:
						InitializeBehaviourComponent<BouncingProjectile>();
						break;
					case BeahviourType.Explosive:
						InitializeBehaviourComponent<ExplosiveProjectile>();
						break;
					default:
						Debug.LogWarning("BehaviourType not found");
						break;
				}
			}
		}

		private void InitializeBehaviourComponent<T>() where T : BehaviourProjectile {
			var behaviourComponent = gameObject.AddComponent<T>();
			behaviourComponent.InitializeComponent(this,settings);
			behaviourComponents.Add(behaviourComponent);
		}
		
		public void RemoveProjectBehaviour(BeahviourType behaviourToRemove) {
			switch (behaviourToRemove) {
				case BeahviourType.Normal:
					
					break;
				case BeahviourType.Bouncing:
					
					break;
				case BeahviourType.Explosive:
					
					break;
				default:
					Debug.LogWarning("BehaviourType not found");
					break;
			}
		}

	}

}