namespace UIFlow
{
    using UnityEngine;

#if UNITY_EDITOR
    using UnityEditor;
#endif

    public class StoryboardEnvironment : MonoBehaviour
    {
        public static StoryboardEnvironment Singleton { get; private set; }

        public Storyboard Storyboard => _storyboard;
        [field: SerializeField, Immutable] private Storyboard _storyboard;

        public bool MakeAsSingletone => _makeAsSingletone;
        [SerializeField] private bool _makeAsSingletone;

        // Methods

        private void Awake()
        {
            if (Singleton != null)
            {
                DestroyImmediate(gameObject);
                return;
            }

            if (_makeAsSingletone)
            {
                Singleton = this;
                DontDestroyOnLoad(gameObject);
            }
        }

        private void OnValidate()
        {
            if(name != "StoryboardEnvironment")
                name = "StoryboardEnvironment";

            if(Storyboard == null)
            {
                Storyboard storyboard = transform.GetComponentInChildren<Storyboard>();
                if(storyboard != null)
                {
                    _storyboard = storyboard;
                    return;
                }

                GameObject obj = new GameObject("Storyboard");
                obj.transform.SetParent(transform);
                obj.transform.localPosition = Vector3.zero;
                obj.transform.rotation = Quaternion.identity;
                obj.transform.localScale = Vector3.one;

                _storyboard = obj.AddComponent<Storyboard>();
            }
        }

        private void Reset()
        {
            OnValidate();
        }

#if UNITY_EDITOR
        [MenuItem("GameObject/UI/Storyboard")]
        public static void Create()
        {
            GameObject obj = new GameObject();
            obj.AddComponent<StoryboardEnvironment>();
        }
#endif
    }
}
