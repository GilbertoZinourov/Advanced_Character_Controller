using System;
using System.Collections.Generic;
using UnityEngine;

namespace Advanced_Weapon_System {
	public enum BehaviourType{Normal,Bouncing,Explosive}
	public enum MovementType{Straight,Aiming,Gravity}

	[Serializable]
	public class Settings {
		public MovementType movementType;
		public List<BehaviourType> behaviourTypes;
		[Header("MovmentSettings")] 
		public MovementSettings movementSettings;
		[Header("BehaviourSettings")] 
		public BouncingSettings bouncingSettings;
		public ExplosiveSettings explosiveSettings;

		public PhysicMaterial gravityMaterial;
		public PhysicMaterial gravityBouncingMaterial;
		
	}
	
	[CreateAssetMenu(menuName = "Data/Settings/ProjectileSettings")]
	public class ProjectileSettings : ScriptableObject {
		public Settings settings;
	}

}