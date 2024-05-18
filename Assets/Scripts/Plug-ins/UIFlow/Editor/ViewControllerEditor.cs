namespace UIFlow.Editors
{
    using System.Linq;
 
    using UnityEngine;
    
    using UnityEditor;

    [CustomEditor(typeof(ViewController), true)]
    public class ViewControllerEditor : Editor
    {
        private ViewController _target;

        private string[] _excludeProperties = new string[]
        {
            "m_Script",
            "_layer",
            "_transition",
            "_unconstrainted"
        };

        // Methods

        private void OnEnable()
        {
            _target = target as ViewController;
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            Base();

            if (HasOtherProperties())
            {
                AdvancedGUI.Headline("Other Properties");
                DrawPropertiesExcluding(serializedObject, _excludeProperties);
            }

            serializedObject.ApplyModifiedProperties();

            if (GUI.changed)
                EditorUtility.SetDirty(target);
        }

        private void Base()
        {
            AdvancedGUI.Headline("Base");

            var layer = serializedObject.FindProperty("_layer");
            EditorGUILayout.PropertyField(layer, true);

            var transition = serializedObject.FindProperty("_transition");
            EditorGUILayout.PropertyField(transition, true);

            var unconstrainted = serializedObject.FindProperty("_unconstrainted");
            EditorGUILayout.PropertyField(unconstrainted, true);

            if (Application.isPlaying)
                EditorGUILayout.LabelField("Section", _target.Section);
        }

        private bool HasOtherProperties()
        {
            var iterator = serializedObject.GetIterator();
            bool enterChildren = true;
            while (iterator.NextVisible(enterChildren))
            {
                enterChildren = false;
                if (!_excludeProperties.Contains(iterator.name))
                    return true;
            }

            return false;
        }
    }
}