using System;
using System.Collections.Generic;
using UnityEngine;

namespace Advanced_Movement{
    public class PlayerMovement : MonoBehaviour
    {
        #region Enumerators

        [Flags]
        public enum Mechanics
        {
            Nothing = 0,
            Base8DirMovement = 1 << 0,
            CrouchAndSlide = 1 << 1,
            GravityAndJump = 1 << 2,
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
            Sliding,
            InAir,
        }

        public enum PressMode{
            Toggle,
            Hold
        }

        #endregion

        public Action OnVariableChange;
        public Action<Vector2> OnMovementInput;
        public Action<Vector2> OnMouseInput;
        public Action OnRunningInputPressed;
        public Action OnRunningInputReleased;
        public Action OnCrouchInputPressed;
        public Action OnCrouchInputReleased;
        
        public Action OnJumpInputPressed;
        
        #region Perspective Variables

        public FirstOrThird desiredPerspective;

        // First person
        public Transform firstPersonCameraPosition;
    
        // Third person
        public Transform centerOfRotation;
        public float radiusOfRotation;
        public Vector3 cameraOffset;

        #endregion

        public CharacterController controller;
        [HideInInspector] public Transform camTransform;
        
        #region Mechanics Init Variables

        public Dictionary<string, bool> mechanicsDictionary = new Dictionary<string, bool>();
        public Mechanics desiredMechanics;
        public PlayerStates currentState;

        #endregion
    
        #region 8 Directional Movement
        
        public float walkSpeed;
        public float runSpeed;
        public bool invertedX, invertedY;
        [Range(50, 350)] public float mouseSensitivity;
        public float minVerticalLookClampValue = -90;
        public float maxVerticalLookClampValue = 90;
        public LayerMask groundMask;
        public PressMode runMode;
        
        private Vector3 _movementDirection;
        
        public Vector3 MovementDirection{
            get => _movementDirection;
            set => _movementDirection = value;
        }

        #endregion

        #region Crouch And Slide
        
        public bool canSlide;
        public PressMode crouchMode;
        public float crouchTime, slideTime;

        #endregion
        
        #region Gravity And Jump Variables

        [Range(4, 16)] public int numberOfGroundedCheckPoints = 4;
        public float radiusOfGroundedCheckPoints;
        public float groundCheckCastDistance = .5f;
        public float gravity = 9.8f;
        public float fallMultiplier = 1.5f;

        public float jumpForce;
        [Range(1, 10)] public int numberOfPossibleJumps = 1;
        public bool canMoveInAir;

        #endregion

        #region Unity Methods

        private void Awake()
        {
            PlaceCamera();
            ControllerInit();
            MechanicsInit();
        }
        
        private void OnValidate(){
            OnVariableChange?.Invoke();
        }

        #endregion

        #region Initialization Methods

        private void MechanicsInit()
        {
            foreach(Mechanics flag in Enum.GetValues(typeof(Mechanics)))
            {
                if (flag <= 0 || mechanicsDictionary.ContainsKey(flag.ToString())) continue;
                mechanicsDictionary.Add(flag.ToString(), (desiredMechanics & flag) > 0);
                Debug.Log(flag + " was added to the dictionary with value: " + mechanicsDictionary[flag.ToString()]);
            }
        }
        
        private void PlaceCamera()
        {
            camTransform = Camera.main.transform;
            if (!camTransform)
            {
                Debug.LogError("MainCamera was ton found!");
                return;
            }
            switch(desiredPerspective)
            {
                case FirstOrThird.First:
                    camTransform.SetParent(firstPersonCameraPosition);
                    camTransform.localPosition = Vector3.zero;
                    camTransform.rotation = Quaternion.Euler(Vector3.zero);
                    break;
                case FirstOrThird.Third:
                    camTransform.SetParent(centerOfRotation);
                    camTransform.rotation = Quaternion.Euler(Vector3.zero);
                    camTransform.position = centerOfRotation.position - centerOfRotation.forward * radiusOfRotation +
                                            cameraOffset;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void ControllerInit()
        {
            controller = GetComponent<CharacterController>();
            if (!controller)
            {
                controller = gameObject.AddComponent<CharacterController>();
            }
        }
        
        #endregion
        
        #region Mechanics Event Methods

        public void Move(Vector2 input){
            OnMovementInput?.Invoke(input.normalized);
        }
        
        public void Rotate(Vector2 input){
            OnMouseInput?.Invoke(input);
        }
        
        public void Running(){
            OnRunningInputPressed?.Invoke();
        }
        
        public void RunningOff(){
            OnRunningInputReleased?.Invoke();
        }
        
        public void Crouching(){
            OnCrouchInputPressed?.Invoke();
        }
        
        public void CrouchingOff(){
            OnCrouchInputReleased?.Invoke();
        }
        
        public void Jump(){
            OnJumpInputPressed?.Invoke();
        }

        #endregion

        #region PlayerStates
        
        public void ChangePlayerState(PlayerStates state)
        {
            if (currentState == state) return;
            currentState = state;
        }
        
        #endregion
        
        #region Gizmos
        
        private void OnDrawGizmosSelected()
        {
            Color defaultColor = Gizmos.color;
            // Gizmos.color = Color.blue;
            // Gizmos.DrawLine(transform.position, transform.position + Vector3.down * 5);
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, transform.position + MovementDirection.normalized);
        
            DrawGravityCheckers(Color.red);
            Gizmos.color = defaultColor;
        }
        
        private void DrawGravityCheckers(Color color)
        {
            Gizmos.color = color;
        
            float gradPerSegment = (float) 360 / numberOfGroundedCheckPoints;
            float currentAngle = 0f;
            for (int i = 0; i < numberOfGroundedCheckPoints; i++)
            {
                float currentAngleInRad = currentAngle * Mathf.Deg2Rad;
                Vector3 pointPosition =
                    transform.position + (new Vector3(Mathf.Cos(currentAngleInRad), 0, Mathf.Sin(currentAngleInRad)) * radiusOfGroundedCheckPoints);
                Gizmos.DrawSphere(pointPosition, .1f);
                Gizmos.DrawLine(pointPosition, pointPosition + Vector3.down * groundCheckCastDistance);
                currentAngle += gradPerSegment;
            }
        }
        
        #endregion
    }
}
