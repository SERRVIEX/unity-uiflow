namespace UIFlow.Editors
{
    using System;
    using System.Linq;
    using System.Reflection;
    using System.Collections.Generic;
  
    using UnityEngine;

    using UnityEditor;

    using Object = UnityEngine.Object;

    [InitializeOnLoad]
    public static class PostCompile
    {
        private static Texture2D _viewControllerIcon;

        // Constructors

        static PostCompile()
        {
            AssemblyReloadEvents.afterAssemblyReload += CollectScripts;

            _viewControllerIcon = Resources.Load<Texture2D>("uiflowkit_view_controller_editor");
        }

        // Methods

        private static void CollectScripts()
        {
            if (_viewControllerIcon == null)
                return;

            List<Type> viewControllerTypes = new List<Type>();

            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach (Assembly assembly in assemblies)
            {
                Type[] types = assembly.GetTypes()
                    .Where(type => type.IsSubclassOf(typeof(ViewController)))
                    .ToArray();

                viewControllerTypes.AddRange(types);
            }

            Object[] viewControllerScripts = Resources.FindObjectsOfTypeAll<MonoScript>()
                .Where(script => script != null && viewControllerTypes.Contains(script.GetClass()))
                .Cast<Object>()
                .ToArray();

            foreach (Object script in viewControllerScripts)
                TryUpdateIcon(script);
        }

        private static void TryUpdateIcon(Object obj)
        {
            var texture = EditorGUIUtility.ObjectContent(null, typeof(Object)).image;
            if (texture == _viewControllerIcon)
                return;

            EditorGUIUtility.SetIconForObject(obj, _viewControllerIcon);
            EditorUtility.SetDirty(obj);
        }
    }
}