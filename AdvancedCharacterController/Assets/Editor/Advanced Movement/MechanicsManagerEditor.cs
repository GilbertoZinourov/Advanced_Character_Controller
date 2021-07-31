using Advanced_Movement;
using UnityEditor;

namespace Editor.Advanced_Movement{
    [CustomEditor(typeof(MechanicsManager))]
    public class MechanicsManagerEditor : UnityEditor.Editor
    {
        private PlayerMovement _playerMovement;

        #region Perspective Variables
    
        private SerializedProperty _desiredPerspective;
        private SerializedProperty _playerHeadPosition;
        private SerializedProperty _centerOfRotation;
        private SerializedProperty _radiusOfRotation;
        private SerializedProperty _cameraOffset;
    
        #endregion

        private void OnEnable(){
            _playerMovement = (PlayerMovement) target;

            _desiredPerspective = serializedObject.FindProperty("desiredPerspective");
            _playerHeadPosition = serializedObject.FindProperty("firstPersonCameraPosition");
            _centerOfRotation = serializedObject.FindProperty("centerOfRotation");
            _radiusOfRotation = serializedObject.FindProperty("radiusOfRotation");
            _cameraOffset = serializedObject.FindProperty("cameraOffset");
        }
        
        public override void OnInspectorGUI()
        {
            serializedObject.Update();
        
            //FirstPreparations();
        
            serializedObject.ApplyModifiedProperties();
        }
    }
}
