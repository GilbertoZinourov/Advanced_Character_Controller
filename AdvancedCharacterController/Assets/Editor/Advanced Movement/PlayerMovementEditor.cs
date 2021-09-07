using System;
using System.Collections.Generic;
using Advanced_Movement;
using UnityEditor;

namespace Editor.Advanced_Movement{
    [CustomEditor(typeof(PlayerMovement))]
    public class PlayerMovementEditor : UnityEditor.Editor
    {
        private PlayerMovement _playerMovement;
        private static Dictionary<string, bool> _foldoutsDictionary = new Dictionary<string, bool>();

        #region Perspective Variables
    
        private SerializedProperty _desiredPerspective;
        private SerializedProperty _playerHeadPosition;
        private SerializedProperty _centerOfRotation;
        private SerializedProperty _radiusOfRotation;
        private SerializedProperty _cameraOffset;
    
        #endregion

        #region 8 Dir Movement Variables

        private SerializedProperty _walkSpeed;
        private SerializedProperty _runSpeed;
        private SerializedProperty _invertedX;
        private SerializedProperty _invertedY;
        private SerializedProperty _mouseSensitivity;
        private SerializedProperty _minLookClamp;
        private SerializedProperty _maxLookClamp;
        private SerializedProperty _groundMask;
        private SerializedProperty _runMode;

        #endregion

        #region CrouchAndSlide
        
        private SerializedProperty _crouchMode;
        private SerializedProperty _crouchTime;
        private SerializedProperty _canSlide;
        private SerializedProperty _slideTime;

        #endregion

        #region Gravity And Jump Variables

        private SerializedProperty _numberOfGroundCheckPoints;
        private SerializedProperty _radiusOfGroundedCheckPoints;
        private SerializedProperty _groundCheckDistance;
        private SerializedProperty _gravity;
        private SerializedProperty _fallMultiplier;
        private SerializedProperty _jumpForce;
        private SerializedProperty _numberOfPossibleJumps;
        private SerializedProperty _fallOffSpeed;
        private SerializedProperty _maxInAirSpeed;

        #endregion

        private void OnEnable()
        {
            _playerMovement = (PlayerMovement) target;
        
            _desiredPerspective = serializedObject.FindProperty("desiredPerspective");
            _playerHeadPosition = serializedObject.FindProperty("firstPersonCameraPosition");
            _centerOfRotation = serializedObject.FindProperty("centerOfRotation");
            _radiusOfRotation = serializedObject.FindProperty("radiusOfRotation");
            _cameraOffset = serializedObject.FindProperty("cameraOffset");
            _walkSpeed = serializedObject.FindProperty("walkSpeed");
            _runSpeed = serializedObject.FindProperty("runSpeed");
            _invertedX = serializedObject.FindProperty("invertedX");
            _invertedY = serializedObject.FindProperty("invertedY");
            _mouseSensitivity = serializedObject.FindProperty("mouseSensitivity");
            _minLookClamp = serializedObject.FindProperty("minVerticalLookClampValue");
            _maxLookClamp = serializedObject.FindProperty("maxVerticalLookClampValue");
            _groundMask = serializedObject.FindProperty("groundMask");
            _runMode = serializedObject.FindProperty("runMode");
            
            _crouchMode = serializedObject.FindProperty("crouchMode");
            _crouchTime = serializedObject.FindProperty("crouchTime");
            _canSlide = serializedObject.FindProperty("canSlide");
            _slideTime = serializedObject.FindProperty("slideTime");
            
            _groundCheckDistance = serializedObject.FindProperty("groundCheckCastDistance");
            _numberOfGroundCheckPoints = serializedObject.FindProperty("numberOfGroundedCheckPoints");
            _radiusOfGroundedCheckPoints = serializedObject.FindProperty("radiusOfGroundedCheckPoints");
            _gravity = serializedObject.FindProperty("gravity");
            _fallMultiplier = serializedObject.FindProperty("fallMultiplier");
            _jumpForce = serializedObject.FindProperty("jumpForce");
            _numberOfPossibleJumps = serializedObject.FindProperty("numberOfPossibleJumps");
            _fallOffSpeed = serializedObject.FindProperty("fallOffSpeed");
            _maxInAirSpeed = serializedObject.FindProperty("maxInAirSpeed");


            CreateDictionary();
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
        
            FirstPreparations();
            MechanicsSetUp();
        
            serializedObject.ApplyModifiedProperties();
        }

        private void CreateDictionary()
        {
            foreach(PlayerMovement.Mechanics flag in Enum.GetValues(typeof(PlayerMovement.Mechanics)))
            {
                if (flag <= 0 || _foldoutsDictionary.ContainsKey(flag.ToString())) continue;
                _foldoutsDictionary.Add(flag.ToString(), false);
                //Debug.Log(flag + " was added to the dictionary");
            }
        }

        private void FirstPreparations()
        {
            PlayerDebugValues();
        
            EditorGUILayout.LabelField("Firs Preparations", EditorStyles.boldLabel);
        
            #region Perspective

            EditorGUILayout.PropertyField(_desiredPerspective);

            EditorGUI.indentLevel++;
            switch (_playerMovement.desiredPerspective)
            {
                case PlayerMovement.FirstOrThird.First:
                    if (!_playerMovement.firstPersonCameraPosition)
                    {
                        EditorGUILayout.HelpBox("Please assign the Players Head Transform.", MessageType.Error);
                    }
                    EditorGUILayout.PropertyField(_playerHeadPosition);
                    break;
                case PlayerMovement.FirstOrThird.Third:
                    if (!_playerMovement.centerOfRotation)
                    {
                        EditorGUILayout.HelpBox("Please assign the Cameras Center Of Rotation.", MessageType.Error);
                    }
                    EditorGUILayout.PropertyField(_centerOfRotation);
                    EditorGUILayout.PropertyField(_radiusOfRotation);
                    EditorGUILayout.PropertyField(_cameraOffset);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            EditorGUI.indentLevel--;

            #endregion
            EditorGUILayout.Space(25);
        }

        private void PlayerDebugValues()
        {
            EditorGUILayout.LabelField("Debug", EditorStyles.boldLabel);
            string state = "";

            switch (_playerMovement.currentState)
            {
                case PlayerMovement.PlayerStates.Idle:
                    state = "Idle";
                    break;
                case PlayerMovement.PlayerStates.Walking:
                    state = "Walking";
                    break;
                case PlayerMovement.PlayerStates.Running:
                    state = "Running";
                    break;
                case PlayerMovement.PlayerStates.InAir:
                    state = "In Air";
                    break;
                case PlayerMovement.PlayerStates.Sliding:
                    state = "Sliding";
                    break;
                case PlayerMovement.PlayerStates.Mantling:
                    state = "Mantling";
                    break;
                case PlayerMovement.PlayerStates.WallRunning:
                    state = "Wall Running";
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            EditorGUILayout.LabelField("Current Player State:  " + state);
            EditorGUILayout.Space(15);
        }

        private void MechanicsSetUp()
        {
            EditorGUILayout.LabelField("Mechanics", EditorStyles.boldLabel);
        
            _playerMovement.desiredMechanics = (PlayerMovement.Mechanics) EditorGUILayout.EnumFlagsField("Desired Mechanics", _playerMovement.desiredMechanics);
            foreach(PlayerMovement.Mechanics flag in Enum.GetValues(typeof(PlayerMovement.Mechanics)))
            {
                if ((_playerMovement.desiredMechanics & flag) <= 0) continue;
                var flagString = flag.ToString();
                
                _foldoutsDictionary[flagString] = EditorGUILayout.Foldout(_foldoutsDictionary[flagString], flag.ToString());
                
                if (_foldoutsDictionary[flagString]){
                    EditorGUI.indentLevel++;
                    switch(flag)
                    {
                        case PlayerMovement.Mechanics.Nothing:
                            break;
                        case PlayerMovement.Mechanics.Base8DirMovement:
                            Show8DirMovementVariables();
                            break;
                        case PlayerMovement.Mechanics.GravityAndJump:
                            ShowGravityAndJumpVariables();
                            break;
                        case PlayerMovement.Mechanics.CrouchAndSlide:
                            ShowCrouchAndSlideVariables();
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                    EditorGUI.indentLevel--;
                }
            }
        }

        private void Show8DirMovementVariables()
        {
            EditorGUILayout.LabelField("Movement Variables", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(_walkSpeed);
            EditorGUILayout.PropertyField(_runSpeed);
            EditorGUILayout.PropertyField(_runMode);
            EditorGUILayout.PropertyField(_groundMask);
            
            EditorGUILayout.Space(10);
            
            EditorGUILayout.LabelField("Mouse Variables", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(_invertedX);
            EditorGUILayout.PropertyField(_invertedY);
            EditorGUILayout.PropertyField(_mouseSensitivity);
            EditorGUILayout.PropertyField(_minLookClamp);
            EditorGUILayout.PropertyField(_maxLookClamp);

            float min = _minLookClamp.floatValue;
            float max = _maxLookClamp.floatValue;
            EditorGUILayout.MinMaxSlider(ref min, ref max, -180, 180);
            _minLookClamp.floatValue = min;
            _maxLookClamp.floatValue = max;
        }

        private void ShowGravityAndJumpVariables()
        {
            EditorGUILayout.LabelField("Gravity Variables", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(_numberOfGroundCheckPoints);
            EditorGUILayout.PropertyField(_radiusOfGroundedCheckPoints);
            EditorGUILayout.PropertyField(_groundMask);
            EditorGUILayout.PropertyField(_groundCheckDistance);
            EditorGUILayout.PropertyField(_gravity);
            EditorGUILayout.PropertyField(_fallMultiplier);
            
            EditorGUILayout.Space(10);
            
            EditorGUILayout.LabelField("Jump Variables", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(_jumpForce);
            EditorGUILayout.PropertyField(_numberOfPossibleJumps);
            
            EditorGUILayout.Space(10);
            
            EditorGUILayout.LabelField("In Air Variables", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(_fallOffSpeed);
            EditorGUILayout.PropertyField(_maxInAirSpeed);
        }

        private void ShowCrouchAndSlideVariables(){
            EditorGUILayout.PropertyField(_crouchMode);
            EditorGUILayout.PropertyField(_crouchTime);
            EditorGUILayout.PropertyField(_canSlide);
            if (_canSlide.boolValue){
                EditorGUI.indentLevel++;
                
                EditorGUILayout.PropertyField(_slideTime);
                EditorGUILayout.PropertyField(_groundMask);
                
                EditorGUI.indentLevel--;
            }
        }
    }
}
