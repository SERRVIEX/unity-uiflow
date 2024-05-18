namespace UIFlow
{
    using System.Collections.Generic;
    
    using UnityEngine;

#if UNITY_EDITOR
    using UnityEditor;
#endif

    [CreateAssetMenu(fileName = "Storyboard Content", menuName = "Storyboard/Content")]
    public class StoryboardContent : ScriptableObject
    {
        [SerializeField] private List<ViewController> _viewControllers = new List<ViewController>();

        // Methods

        public T GetViewController<T>() where T : ViewController
        {
            for (int i = 0; i < _viewControllers.Count; i++)
                if (_viewControllers[i].GetType() == typeof(T))
                    return (T)_viewControllers[i];

            return null;
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            // Clear the existing list.
            _viewControllers.Clear();

            // Find all assets of type ViewController.
            string[] paths = AssetDatabase.GetAllAssetPaths();

            foreach (string path in paths)
            {
                ViewController viewController = AssetDatabase.LoadAssetAtPath<ViewController>(path);
                if (viewController != null)
                    _viewControllers.Add(viewController);
            }

            EditorUtility.SetDirty(this);
        }
#endif
    }
}