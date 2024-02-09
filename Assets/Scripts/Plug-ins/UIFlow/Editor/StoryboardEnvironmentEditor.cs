namespace UIFlow
{
    using UnityEngine;

    using UnityEditor;

    [CustomEditor(typeof(StoryboardEnvironment), true)]
    public class StoryboardEnvironmentEditor : Editor
    {
        private StoryboardEnvironment _target;

        // Methods

        private void OnEnable()
        {
            _target = target as StoryboardEnvironment;
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            AdvancedGUI.Headline("Settings");

            SerializedProperty storyboard = serializedObject.FindProperty("_storyboard");
            EditorGUILayout.PropertyField(storyboard, true);

            
            SerializedProperty makeAsSingletone = serializedObject.FindProperty("_makeAsSingletone");
            EditorGUILayout.PropertyField(makeAsSingletone, true);

            if(_target.MakeAsSingletone)
                EditorGUILayout.HelpBox("Keep this storyboard across all scenes (this object is not destroyed on scene changing).", MessageType.Info);
            else
                EditorGUILayout.HelpBox("Each scene must have its storyboard.", MessageType.Info);

            serializedObject.ApplyModifiedProperties();

            if (GUI.changed)
                EditorUtility.SetDirty(target);
        }
    }
}