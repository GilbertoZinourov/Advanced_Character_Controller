using System;
using UnityEngine;

namespace Advanced_Movement{
    public class MechanicsManager : MonoBehaviour
    {
        #region Enumerators

        [Flags]
        public enum Mechanics
        {
            Nothing = 0,
            Base8DirMovement = 1 << 0,
            Gravity = 1 << 1,
            Jump = 1 << 2,
        }
    
        public enum FirstOrThird
        {
            First,
            Third
        }

        public enum PlayerStates
        {
            Idle,
            Walking,
            Running,
            Falling,
        }

        #endregion

        #region Perspective Variables

        public FirstOrThird desiredPerspective;
        private Transform _camTransform;
    
        // First person
        [Tooltip("The Transform to witch the MainCamera will snap to at the start of the game")]
        public Transform firstPersonCameraPosition;
    
        // Third person
        [Tooltip("The Transform around witch the MainCamera will rotate during the game")]
        public Transform centerOfRotation;
        public float radiusOfRotation;
        public Vector3 cameraOffset;

        #endregion
    }
}
