namespace UIFlow
{
    using UnityEngine;

    using UnityEditor;
    using static System.Collections.Specialized.BitVector32;

    [CustomEditor(typeof(Storyboard), true)]
    public class StoryboardEditor : Editor
    {
        private Storyboard _target;

        // Methods

        private void OnEnable()
        {
            _target = target as Storyboard;
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            Base();
            Sections();
            Canvas();
            ViewControllers();

            serializedObject.ApplyModifiedProperties();

            if (GUI.changed)
                EditorUtility.SetDirty(target);
        }

        private void Base()
        {
            AdvancedGUI.Headline("Base");

            SerializedProperty camera = serializedObject.FindProperty("_camera");
            EditorGUILayout.PropertyField(camera, true);

            if(camera.objectReferenceValue == null)
            {
                if(Camera.main != null)
                {
                    if (GUILayout.Button("Link MainCamera"))
                        _target.SetMainCameraAsDefault();
                }
                if(GUILayout.Button("New Camera"))
                    _target.NewCamera();

                GUILayout.Space(10);
            }

            SerializedProperty audioListener = serializedObject.FindProperty("_audioListener");
            EditorGUILayout.PropertyField(audioListener, true);

            SerializedProperty events = serializedObject.FindProperty("_eventSystem");
            EditorGUILayout.PropertyField(events, true);
        }

        private void Sections()
        {
            AdvancedGUI.Headline("Sections");

            if (Application.isPlaying)
            {
                foreach (var section in _target.Sections)
                {
                    EditorGUILayout.LabelField(section.Key);
                    EditorGUI.indentLevel++;
                    for (int i = 0; i < section.Value.Count; i++)
                    {
                        GUI.color = section.Value[i].gameObject.activeSelf ? Color.white : Color.gray;
                        EditorGUILayout.LabelField("↳" + section.Value[i].GetType().Name);
                    }
                    GUI.color = Color.white;
                    EditorGUI.indentLevel--;
                }
            }
        }

        private void Canvas()
        {
            AdvancedGUI.Headline("Canvas");

            SerializedProperty canvasScaler = serializedObject.FindProperty("_canvasScaler");
            EditorGUILayout.PropertyField(canvasScaler, true);

            SerializedProperty layers = serializedObject.FindProperty("_layers");
            EditorGUILayout.PropertyField(layers, true);

            SerializedProperty initialSection = serializedObject.FindProperty("_initialSection");
            EditorGUILayout.PropertyField(initialSection, true);
        }

        private void ViewControllers()
        {
            AdvancedGUI.Headline("View Controllers");

            SerializedProperty initialViewController = serializedObject.FindProperty("_initialViewController");
            EditorGUILayout.PropertyField(initialViewController, true);

            //SerializedProperty cachedViewControllers = serializedObject.FindProperty("_cachedViewControllers");
            //EditorGUILayout.PropertyField(cachedViewControllers, true);
        }
    }
}