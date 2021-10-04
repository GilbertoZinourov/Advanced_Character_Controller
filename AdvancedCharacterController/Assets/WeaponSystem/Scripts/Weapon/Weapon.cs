using System;
using System.Collections.Generic;
using UnityEngine;

namespace Advanced_Weapon_System {

	public class EquippedPowerUp {
		public TogglePowerUp powerUp;
		public bool equipped;

		public EquippedPowerUp(TogglePowerUp power, bool equip) {
			powerUp = power;
			equipped = equip;
		}
	}
	
	public class Weapon : MonoBehaviour {
		[SerializeField] private WeaponSettings weaponSettings;
		[SerializeField] private ProjectileSettings projectileSettings;
		private Projectile projectile;
		public GameObject projectilePrefab;
		public Transform firePoint;
		private GameObject projectileInstance;
		public LineController lineController;
		public List<Transform> laserPoints=new List<Transform>();

		public UIPowerUp uiPowerUp;

		private List<EquippedPowerUp> powerUps=new List<EquippedPowerUp>();

		public WeaponSettings WeaponSettings {
			get => weaponSettings;
			set {
				weaponSettings = value;
				Init();
			}
		}
		public ProjectileSettings ProjectileSettings {
			get => projectileSettings;
			set { 
				projectileSettings = value;
				Init();
			}
		}
		public Projectile Projectile {
			get => projectile;
			set => projectile = value;
		}
		
		private int currentBullet;
		public int CurrentBullet => currentBullet;

		private float timer = 0;
		public float Timer {
			get {
				if (timer > 1 / weaponSettings.fireRate) {
					return 1 / weaponSettings.fireRate;
				}
				return timer;
			}
		}

		private bool canFire = false;
		private bool alreadyFired = false;
		private int fireRafficCount = 0;

		public event Action Fired;
		public event Action TypeChanged;
		public event Action Initialized;
		public event Action FireRateChanged;
		public event Action BulletChange;

		public void Init() {
			//projectileInstance=(GameObject)PrefabUtility.InstantiatePrefab(projectilePrefab, transform);
			projectileInstance = Instantiate(projectilePrefab, firePoint.transform.position, transform.rotation, transform);
			Projectile = projectileInstance.GetComponent<Projectile>();
			Projectile.AddBehaviours(projectileSettings);
			timer = 1 / weaponSettings.fireRate;
			canFire = true;
			fireRafficCount = 0;
			currentBullet = weaponSettings.maxBullets;
			AddLaserPoints();
			Initialized?.Invoke();
		}

		private void Update() {
			if (timer < 1 / weaponSettings.fireRate) {
				timer += Time.deltaTime;
			}
			else {
				if (currentBullet > 0) {
					canFire = true;
				}
			}
			
			var position = transform.position + transform.forward * 0.5f;
			if (weaponSettings.showBouncingPreview) {
				if (Projectile && Projectile.projectileSettings != null && Projectile.projectileSettings.bouncingSettings != null) {
					BouncingSettingsData bouncingSettings = Projectile.projectileSettings.bouncingSettings;
					DrawLine(position, transform.forward, bouncingSettings.maxReflectionCount, bouncingSettings.maxReflectionCount, bouncingSettings.maxStepDistance);
				}
			}
			else {
				DrawLine(position, transform.forward, 0,0,500);
			}
		}

		private void AddLaserPoints() {
			laserPoints.Clear();
			laserPoints.Add(firePoint);
			if (weaponSettings.showBouncingPreview) {
				for (int i = 0; i < Projectile.projectileSettings.bouncingSettings.maxReflectionCount+1; i++) {
					var go = new GameObject($"LaserPoint-{i+1}");
					go.transform.SetParent(lineController.transform);
					//Instantiate(go,lineController.transform);
					laserPoints.Add(go.transform);
				}
			}
			else {
				var go = new GameObject($"LaserPoint-{1}");
				go.transform.SetParent(lineController.transform);
				//Instantiate(go,lineController.transform);
				laserPoints.Add(go.transform);
			}
			lineController.SetLine(laserPoints.ToArray());
		}

		public void FireWithWeapon() {
			if (canFire) {
				switch (weaponSettings.fireType) {
					case FireType.Muanual:
						if (!alreadyFired) {
							Fire();
						}
						break;
					case FireType.SemiAutomatic:
						if (fireRafficCount < weaponSettings.fireRaffic) {
							Fire();
							fireRafficCount++;
						}
						break;
					case FireType.Automatic:
						Fire();
						break;
				}
			}
		}

		public void ChangeWeaponType() {
			switch (weaponSettings.fireType) {
				case FireType.Muanual:
					weaponSettings.fireType = FireType.SemiAutomatic;
					break;
				case FireType.SemiAutomatic:
					weaponSettings.fireType = FireType.Automatic;
					break;
				case FireType.Automatic:
					weaponSettings.fireType = FireType.Muanual;
					break;
			}
			TypeChanged?.Invoke();
		}

		public void ResetRafficCount() {
			fireRafficCount = 0;
			alreadyFired = false;
		}

		public void Recharge() {
			currentBullet = weaponSettings.maxBullets;
			BulletChange?.Invoke();
		}

		private void Fire() {
			var obj = Instantiate(projectileInstance, firePoint.transform.position, transform.rotation);
			obj.SetActive(true);
			canFire = false;
			alreadyFired = true;
			timer = 0;
			currentBullet--;
			Fired?.Invoke();
			BulletChange?.Invoke();
		}

		public void AddMaxAmmo(int maxAmmo) {
			weaponSettings.maxBullets += maxAmmo;
			currentBullet += maxAmmo;
			BulletChange?.Invoke();
		}

		public void AddFireRate(float fireRate) {
			weaponSettings.fireRate += fireRate;
			FireRateChanged?.Invoke();
		}

		public void AddPowerUp(TogglePowerUp powerUp) {
			powerUp.Apply();
			powerUps.Add(new EquippedPowerUp(powerUp,true));
			uiPowerUp.CreatePowerUp(powerUp);
		}

		public void EquipPowerUp(int index) {
			powerUps[index].powerUp.Apply();
		}

		public void RevertPowerUp(int index) {
			powerUps[index].powerUp.Revert();
		}

		private void DrawLine(Vector3 position, Vector3 direction,int totalReflections, int reflectionsRemaining,float maxDistance) {
			Ray ray = new Ray(position, direction);
			RaycastHit hit;
			if (Physics.Raycast(ray, out hit, maxDistance)) {
				direction = Vector3.Reflect(direction, hit.normal);
				position = hit.point;
			}
			else {
				position += direction * maxDistance;
			}
			laserPoints[totalReflections+1 - reflectionsRemaining].transform.position = position;
			if (reflectionsRemaining == 0) {
				return;
			}
			DrawLine(position, direction, totalReflections,reflectionsRemaining - 1,maxDistance);
		}
	}
}