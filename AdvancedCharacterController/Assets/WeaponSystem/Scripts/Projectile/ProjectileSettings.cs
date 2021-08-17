﻿using System;
using System.Collections.Generic;
using UnityEngine;

namespace Advanced_Weapon_System {

	[Serializable]
	public class ProjectileSettings {
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

}