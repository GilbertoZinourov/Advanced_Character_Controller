using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace Advanced_Weapon_System {

	public class Projectile : MonoBehaviour {
		public ProjectileSettings projectileSettings;
		public Rigidbody rb;
		public Collider collider;

		[HideInInspector] public MovementProjectile movementComponent;
		private List<BehaviourProjectile> behaviourComponents=new List<BehaviourProjectile>();

		public event Action<Collision> Hit;

		[SerializeField]private bool destroyOnFirstHit=true;
		[SerializeField]private float delayedDestruction=0;
		[SerializeField]private bool hasBouncingComponent=false;
		
		private float timer=0;
		public float Timer => timer;
		

		private void Init() {
			timer = 0;
			rb ??= GetComponent<Rigidbody>();
			collider ??= GetComponent<Collider>();
		}

		private void Update() {
			timer += Time.deltaTime;
			//Da togliere questa riga quando si farà un input di powerup
			if (timer > projectileSettings.movementSettings.lifeTime) {
				DestroyProjectile(true);
			}
		}

		private void OnBecameInvisible() {
			DestroyProjectile(true);
		}

		private void OnCollisionEnter(Collision other) {
			Debug.Log("Hit");
			Hit?.Invoke(other);
			if (destroyOnFirstHit) {
				StartCoroutine(DestroyProjectile());
			}
		}

		public IEnumerator DestroyProjectile(bool instant=false) {
			yield return null;
			StopProjectile();
			if (instant) {
				Destroy(gameObject);
				yield break;
			}
			float timer = 0;
			while (timer < delayedDestruction) {
				timer += Time.deltaTime;
				yield return null;
			}
			Destroy(gameObject);
		}

		public void StopProjectile() {
			movementComponent.StopProjectile();
			var rigidBody = GetComponent<Rigidbody>();
			rigidBody.useGravity = false;
			rigidBody.velocity=Vector3.zero;
		}
		
		public void AddBehaviours() {
			Init();
			movementComponent = null;
			behaviourComponents.Clear();
			AddProjectileBehaviours();
			AddMovementBehaviour();
		}

		private void AddMovementBehaviour() {
			switch(projectileSettings.movementType)
			{
				case MovementType.Straight:
					InitializeMovementComponent<StraightProjectile>();
					break;
				case MovementType.Aiming:
					InitializeMovementComponent<AimingProjectile>();
					break;
				case MovementType.Gravity:
					if (hasBouncingComponent) {
						collider.material = projectileSettings.gravityBouncingMaterial;
					}
					else {
						collider.material = projectileSettings.gravityMaterial;
					}
					rb.useGravity = true;
					InitializeMovementComponent<GravityProjectile>();
					break;
				default:
					Debug.LogWarning("MovementType not found");
					break;
			}
		}

		private void InitializeMovementComponent<T>() where T : MovementProjectile {
			movementComponent = gameObject.AddComponent<T>();
			movementComponent.InitializeComponent(this);
			if (gameObject.activeSelf) {
				movementComponent.Init();
			}
		}

		private void AddProjectileBehaviours() {
			foreach (BehaviourType projectileSettingsBeahviourType in projectileSettings.behaviourTypes) {
				switch (projectileSettingsBeahviourType) {
					case BehaviourType.Normal:
						InitializeBehaviourComponent<NormalProjectile>();
						break;
					case BehaviourType.Bouncing:
						destroyOnFirstHit = false;
						hasBouncingComponent = true;
						InitializeBehaviourComponent<BouncingProjectile>();
						break;
					case BehaviourType.Explosive:
						delayedDestruction += projectileSettings.explosiveSettings.explosionDelay;
						delayedDestruction += projectileSettings.explosiveSettings.explosionDuration;
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
			behaviourComponent.InitializeComponent(this);
			if (gameObject.activeSelf) {
				behaviourComponent.Init();
			}
			behaviourComponents.Add(behaviourComponent);
		}
		
		// public void RemoveProjectBehaviour(BehaviourType behaviourToRemove) {
		// 	switch (behaviourToRemove) {
		// 		case BehaviourType.Normal:
		// 			
		// 			break;
		// 		case BehaviourType.Bouncing:
		// 			
		// 			break;
		// 		case BehaviourType.Explosive:
		// 			
		// 			break;
		// 		default:
		// 			Debug.LogWarning("BehaviourType not found");
		// 			break;
		// 	}
		// }

	}

}