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
		private Rigidbody rb;

		[SerializeField]private MovementProjectile movementComponent;
		private List<BehaviourProjectile> behaviourComponents=new List<BehaviourProjectile>();

		public event Action<Collision> Hit;

		[SerializeField]private bool destroyOnFirstHit=true;
		[SerializeField]private float delayedDestruction=0;
		
		private float timer=0;
		public float Timer => timer;

		private void Awake() {
			Init();
		}

		private void OnEnable() {
			Init();
		}

		private void Update() {
			timer += Time.deltaTime;
			//Da togliere questa riga quando si farà un input di powerup
			settings = projectileSettings.settings;
			if (timer > settings.movementSettings.lifeTime) {
				Destroy(gameObject);
			}
		}

		private void OnBecameInvisible() {
			Destroy(gameObject);
		}

		private void OnCollisionEnter(Collision other) {
			Debug.Log("Hit");
			Hit?.Invoke(other);
			if (destroyOnFirstHit) {
				StartCoroutine(DestroyProjectile());
			}
		}

		public IEnumerator DestroyProjectile() {
			float timer = 0;
			while (timer < delayedDestruction) {
				movementComponent.StopProjectile();
				timer += Time.deltaTime;
				yield return null;
			}
			Destroy(gameObject);
		}
		
		private void Init() {
			timer = 0;
			settings = projectileSettings.settings;
			rb ??= GetComponent<Rigidbody>();
		}
		
		public void AddBehaviours() {
			Init();
			movementComponent = null;
			behaviourComponents.Clear();
			AddMovementBehaviour();
			AddProjectileBehaviours();
		}

		private void AddMovementBehaviour() {
			switch(settings.movementType)
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
			movementComponent.InitializeComponent(this);
		}

		private void AddProjectileBehaviours() {
			foreach (BehaviourType projectileSettingsBeahviourType in settings.behaviourTypes) {
				switch (projectileSettingsBeahviourType) {
					case BehaviourType.Normal:
						InitializeBehaviourComponent<NormalProjectile>();
						break;
					case BehaviourType.Bouncing:
						destroyOnFirstHit = false;
						InitializeBehaviourComponent<BouncingProjectile>();
						break;
					case BehaviourType.Explosive:
						delayedDestruction += settings.explosiveSettings.explosionDelay;
						delayedDestruction += settings.explosiveSettings.explosionDuration;
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
			behaviourComponents.Add(behaviourComponent);
		}
		
		public void RemoveProjectBehaviour(BehaviourType behaviourToRemove) {
			switch (behaviourToRemove) {
				case BehaviourType.Normal:
					
					break;
				case BehaviourType.Bouncing:
					
					break;
				case BehaviourType.Explosive:
					
					break;
				default:
					Debug.LogWarning("BehaviourType not found");
					break;
			}
		}

	}

}