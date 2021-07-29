using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Advanced_Weapon_System {
	
	public enum BeahviourType{Normal,Bouncing,Explosive}
	public enum MovementType{Straight,Aiming,Gravity}

	public class Settings {
		[SerializeField] private MovementType movementType;
		[SerializeField] private List<BeahviourType> beahviourTypes;
		[SerializeField] private AnimationCurve graphic;
		[SerializeField] private float lifeTime;
		public MovementType MovementType => movementType;
		public List<BeahviourType> BeahviourTypes => beahviourTypes;
		public AnimationCurve Graphic => graphic;
		public float LifeTime => lifeTime;
	}
	[CreateAssetMenu(menuName = "Data/Settings/ProjectileSettings")]
	public class ProjectileSettings : ScriptableObject {
		public Settings settings;
	}

}