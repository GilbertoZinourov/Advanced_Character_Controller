using System;
using System.Collections.Generic;
using UnityEngine;

namespace Advanced_Weapon_System {

	[CreateAssetMenu(menuName = "Data/Settings/ProjectileSettings")]
	public class ProjectileSettings: ScriptableObject {
		public MovementType movementType;
		public List<BehaviourType> behaviourTypes;
		[Header("MovmentSettings")] 
		public MovementSettings movementSettings;
		[Header("BehaviourSettings")] 
		public BouncingSettings bouncingSettings;
		public ExplosiveSettings explosiveSettings;

		public float bulletDamage;
		
		public PhysicMaterial gravityMaterial;
		public PhysicMaterial gravityBouncingMaterial;
		
	}

}