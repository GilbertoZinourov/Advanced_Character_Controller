using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Advanced_Weapon_System {

    [System.Serializable]
    public class ProjectileSettingsData {
        public MovementType movementType;
        public List<BehaviourType> behaviourTypes;
        [Header("MovementSettings")] 
        public MovementSettingsData movementSettings;
        [Header("BehaviourSettings")] 
        public BouncingSettingsData bouncingSettings;
        public ExplosiveSettingsData explosiveSettings;

        public float bulletDamage;
		
        public PhysicMaterial gravityMaterial;
        public PhysicMaterial gravityBouncingMaterial;

        public ProjectileSettingsData(ProjectileSettings settings) {
            movementType = settings.movementType;
            behaviourTypes = settings.behaviourTypes;
            movementSettings = new MovementSettingsData(settings.movementSettings);
            bouncingSettings = new BouncingSettingsData(settings.bouncingSettings);
            explosiveSettings = new ExplosiveSettingsData(settings.explosiveSettings);
            bulletDamage = settings.bulletDamage;
            gravityMaterial = settings.gravityMaterial;
            gravityBouncingMaterial = settings.gravityBouncingMaterial;
        }
    }

    [System.Serializable]
    public class MovementSettingsData {
        public AnimationCurve graphic;
        public float lifeTime;

        public MovementSettingsData(MovementSettings settings) {
            graphic = settings.settingsData.graphic;
            lifeTime = settings.settingsData.lifeTime;
        }
        public MovementSettingsData(MovementSettingsData settings) {
            graphic = settings.graphic;
            lifeTime = settings.lifeTime;
        }
        public MovementSettingsData(AnimationCurve curve,float time) {
            graphic = curve;
            lifeTime = time;
        }
    }
    
    [System.Serializable]
    public class BouncingSettingsData {
        public int maxReflectionCount = 5;
        public float maxStepDistance = 200;

        public BouncingSettingsData(BouncingSettings settings) {
            maxReflectionCount = settings.settingsData.maxReflectionCount;
            maxStepDistance = settings.settingsData.maxStepDistance;
        }
        public BouncingSettingsData(BouncingSettingsData settings) {
            maxReflectionCount = settings.maxReflectionCount;
            maxStepDistance = settings.maxStepDistance;
        }
        public BouncingSettingsData(int maxReflection,float maxStep) {
            maxReflectionCount = maxReflection;
            maxStepDistance = maxStep;
        }
    }
    
    [System.Serializable]
    public class ExplosiveSettingsData {
        public float smallRadius;
        public float smallRadiusDamage;
        public float bigRadius;
        public float bigRadiusDamage;

        public GameObject explosivePrefab;
		
        public float explosionDelay;
        public float explosionDuration;

        public ExplosiveSettingsData(ExplosiveSettings settings) {
            smallRadius = settings.settingsData.smallRadius;
            smallRadiusDamage = settings.settingsData.smallRadiusDamage;
            bigRadius = settings.settingsData.bigRadius;
            bigRadiusDamage = settings.settingsData.bigRadiusDamage;
            explosivePrefab = settings.settingsData.explosivePrefab;
            explosionDelay = settings.settingsData.explosionDelay;
            explosionDuration = settings.settingsData.explosionDuration;
        }
        public ExplosiveSettingsData(ExplosiveSettingsData settings) {
            smallRadius = settings.smallRadius;
            smallRadiusDamage = settings.smallRadiusDamage;
            bigRadius = settings.bigRadius;
            bigRadiusDamage = settings.bigRadiusDamage;
            explosivePrefab = settings.explosivePrefab;
            explosionDelay = settings.explosionDelay;
            explosionDuration = settings.explosionDuration;
        }
    }

}
