namespace UIFlow
{
    using System.Collections;
    using System.Collections.Generic;

    using UnityEngine;
    using UnityEngine.UI;
    using UnityEngine.Assertions;
    using UnityEngine.EventSystems;

    using UIFlow.Core;
    using UIFlow.Utils;

    [RequireComponent(typeof(CanvasScaler))]
    public sealed class Storyboard : MonoBehaviour
    {
        public static Storyboard Instance { get; private set; }

        public static float AspectRatiofactor => CanvasScaler.referenceResolution.x / Screen.width;

        public static Camera Camera => Instance._camera;
        [SerializeField] private Camera _camera;

        [SerializeField] public AudioListener AudioListener => _audioListener;
        [SerializeField] public AudioListener _audioListener;

        public static EventSystem EventSystem => Instance._eventSystem;
        [SerializeField, Immutable] private EventSystem _eventSystem;

        public static CanvasScaler CanvasScaler => Instance._canvasScaler;
        [SerializeField, Immutable] private CanvasScaler _canvasScaler;
        [SerializeField] private Layers _layers;

        public static string ActiveSection
        {
            get => _activeSection;
            set
            {
                if (value == _activeSection)
                    return;

                _activeSection = value;

                foreach (var item in Instance.Sections)
                {
                    if (item.Key == value)
                        item.Value[item.Value.Count - 1].gameObject.SetActive(true);
               
                    else
                    {
                        for (int i = 0; i < item.Value.Count; i++)
                            item.Value[i].gameObject.SetActive(false);
                    }
                }
            }
        }

        private static string _activeSection;

        [SerializeField] private string _initialSection = "default";
        public Dictionary<string, List<ViewController>> Sections = new Dictionary<string, List<ViewController>>();

        [SerializeField] private ViewController _initialViewController;
        private StoryboardContent _storyboardContent;

        private float _eventSystemCooldown;

        // Methods

        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;

            _activeSection = _initialSection;

            Assert.IsNotNull(_camera);
            Assert.IsNotNull(_eventSystem);

            Assert.IsNotNull(_canvasScaler);

            Assert.IsNotNull(_layers.Under);
            Assert.IsNotNull(_layers.Base);
            Assert.IsNotNull(_layers.Extra);
            Assert.IsNotNull(_layers.Context);
            Assert.IsNotNull(_layers.Alert);
            Assert.IsNotNull(_layers.Over);

            _storyboardContent = Resources.LoadAll<StoryboardContent>("")[0];

            AdaptCanvas();

            if (_initialViewController != null)
                Present(_initialViewController);
        }

        private void Update()
        {
            if (_eventSystem.enabled == false)
            {
                _eventSystemCooldown -= Time.unscaledDeltaTime;
                if (_eventSystemCooldown < 0)
                    _eventSystem.enabled = true;
            }
        }

        /// <summary>
        /// Adapt canvas for different resolutions.
        /// </summary>
        private void AdaptCanvas()
        {
            if (SystemUtils.Device == SystemUtils.DeviceType.Phone)
                _canvasScaler.matchWidthOrHeight = 0;
            else
            {
                _canvasScaler.matchWidthOrHeight = 0;

                if (GetScreenOrientation() == ScreenOrientation.Portrait)
                    _canvasScaler.referenceResolution = new Vector2(1080, Screen.height);
                else
                    _canvasScaler.referenceResolution = new Vector2(1920, 1080);
            }
        }

        /// <summary>
        /// Retrieves the current screen orientation, considering both editor and device settings.
        /// </summary>
        /// <remarks>
        /// In the Unity Editor, this method determines the screen orientation based on the aspect ratio 
        /// of the editor window. On a device, it returns the actual device orientation.
        /// </remarks>
        /// <returns>The current screen orientation.</returns>
        private ScreenOrientation GetScreenOrientation()
        {
#if UNITY_EDITOR
            return Screen.height > Screen.width ? ScreenOrientation.Portrait : ScreenOrientation.LandscapeLeft;
#else
            return Screen.orientation;
#endif
        }

        /// <summary>
        /// Retrieves the canvas object associated with the specified layer.
        /// </summary>
        /// <param name="layer">The layer to retrieve the canvas for.</param>
        /// <returns>The canvas object corresponding to the specified layer.</returns>
        public static Canvas GetLayer(Layer layer)
        {
            return layer switch
            {
                Layer.Under => Instance._layers.Under,
                Layer.Base => Instance._layers.Base,
                Layer.Extra => Instance._layers.Extra,
                Layer.Context => Instance._layers.Context,
                Layer.Alert => Instance._layers.Alert,
                Layer.Over => Instance._layers.Over,
                _ => Instance._layers.Under,
            };
        }

        public static ViewController GetViewController(string section, int index)
        {
            return Instance.Sections[section][index];
        }

        /// <summary>
        /// Retrieves the previous view controller relative to the specified view controller.
        /// </summary>
        /// <param name="viewController">The target view controller.</param>
        /// <returns>The previous view controller if found; otherwise, null.</returns>
        public static ViewController GetPreviousViewController(ViewController viewController)
        {
            var controllers = Instance.Sections[viewController.Section];
            if (controllers.Count == 1)
                return null;

            for (int i = controllers.Count - 1; i >= 0; i--)
            {
                if (i == 0) return null;

                if (controllers[i] == viewController)
                    return controllers[i - 1];
            }

            return null;
        }

        /// <summary>
        /// Presents a view controller instantiated from a prefab in the active section's canvas.
        /// </summary>
        /// <typeparam name="T">Type of the view controller to present.</typeparam>
        /// <param name="viewController">The view controller prefab to present.</param>
        /// <param name="blockRaycast">Determines whether to block raycasts to objects underneath the view controller.</param>
        /// <returns>The presented view controller.</returns>
        public static T Present<T>(T viewController, bool blockRaycast = true) where T : ViewController
        {
            return Present(viewController, Instance._layers.GetCanvas(viewController.Layer).transform, blockRaycast);
        }

        /// <summary>
        /// Presents a view controller instantiated from a prefab in the specified section's canvas.
        /// </summary>
        /// <typeparam name="T">Type of the view controller to present.</typeparam>
        /// <param name="prefab">The view controller prefab to present.</param>
        /// <param name="section">The name of the section to present the view controller in.</param>
        /// <param name="blockRaycast">Determines whether to block raycasts to objects underneath the view controller.</param>
        /// <returns>The presented view controller.</returns>
        public static T Present<T>(T prefab, string section, bool blockRaycast = true) where T : ViewController
        {
            Assert.IsFalse(prefab == null);

            var viewController = Instantiate(prefab, Instance._layers.GetCanvas(prefab.Layer).transform);
            viewController.name = viewController.GetType().Name;
            Instance.StartCoroutine(PresentImpl(viewController, section, blockRaycast));

            return (T)viewController;
        }

        /// <summary>
        /// Presents a view controller instantiated from a prefab within another parent transform.
        /// </summary>
        /// <typeparam name="T">Type of the view controller to present.</typeparam>
        /// <param name="prefab">The view controller prefab to present.</param>
        /// <param name="parent">The transform to parent the instantiated view controller to.</param>
        /// <param name="blockRaycast">Determines whether to block raycasts to objects underneath the view controller.</param>
        /// <returns>The presented view controller.</returns>
        public static T Present<T>(T prefab, Transform parent, bool blockRaycast = true) where T : ViewController
        {
            Assert.IsFalse(prefab == null);

            var viewController = Instantiate(prefab, parent);
            viewController.name = viewController.GetType().Name;
            Instance.StartCoroutine(PresentImpl(viewController, null, blockRaycast));

            return (T)viewController;
        }

        /// <summary>
        /// Presents a view controller of the specified type in the active section's canvas.
        /// </summary>
        /// <typeparam name="T">Type of the view controller to present.</typeparam>
        /// <param name="blockRaycast">Determines whether to block raycasts to objects underneath the view controller.</param>
        /// <returns>The presented view controller.</returns>
        public static T Present<T>(bool blockRaycast = true) where T : ViewController
        {
            return Present<T>(string.Empty, blockRaycast);
        }

        /// <summary>
        /// Presents a view controller of the specified type in the specified section's canvas.
        /// </summary>
        /// <typeparam name="T">Type of the view controller to present.</typeparam>
        /// <param name="section">The name of the section to present the view controller in.</param>
        /// <param name="blockRaycast">Determines whether to block raycasts to objects underneath the view controller.</param>
        /// <returns>The presented view controller.</returns>
        public static T Present<T>(string section, bool blockRaycast = true) where T : ViewController
        {
            var prefab = Instance._storyboardContent.GetViewController<T>();
            Assert.IsFalse(prefab == null);

            var viewController = Instantiate(prefab, Instance._layers.GetCanvas(prefab.Layer).transform);
            viewController.name = viewController.GetType().Name;
            Instance.StartCoroutine(PresentImpl(viewController, section, blockRaycast));

            return (T)viewController;
        }

        /// <summary>
        /// Presents a view controller of the specified type within another parent transform.
        /// </summary>
        /// <typeparam name="T">Type of the view controller to present.</typeparam>
        /// <param name="parent">The transform to parent the instantiated view controller to.</param>
        /// <param name="blockRaycast">Determines whether to block raycasts to objects underneath the view controller.</param>
        /// <returns>The presented view controller.</returns>
        public static T Present<T>(Transform parent, bool blockRaycast = true) where T : ViewController
        {
            var prefab = Instance._storyboardContent.GetViewController<T>();
            Assert.IsFalse(prefab == null);

            var viewController = Instantiate(prefab, parent);
            viewController.name = viewController.GetType().Name;
            Instance.StartCoroutine(PresentImpl(viewController, null, blockRaycast));

            return (T)viewController;
        }

        /// <summary>
        /// Coroutine to handle the presentation of a view controller.
        /// </summary>
        /// <param name="viewController">The view controller being presented.</param>
        /// <param name="section">The section where the view controller is being presented.</param>
        /// <param name="blockRaycast">Determines whether to block raycasts during the presentation.</param>
        private static IEnumerator PresentImpl(ViewController viewController, string section, bool blockRaycast)
        {
            if (blockRaycast)
                SetEventSystemInactive(viewController.Transition.Appear);

            string validSection = string.IsNullOrEmpty(section) ? ActiveSection : section;
            if (!Instance.Sections.ContainsKey(validSection))
                Instance.Sections.Add(validSection, new List<ViewController>());

            if (!viewController.Unconstrainted)
                Instance.Sections[validSection].Add(viewController);

            viewController.InitializeController(validSection);
            viewController.Canvas.worldCamera = Instance._camera;

            viewController.OnWillAppear();
            viewController.OnWillAppearHandler?.Invoke();

            viewController.Canvas.enabled = false;
            yield return new WaitForEndOfFrame();
            viewController.Canvas.enabled = true;

            viewController.OnPresentTransition();

            yield return new WaitForSecondsRealtime(viewController.Transition.Appear);

            viewController.OnDidAppear();
            viewController.OnDidAppearHandler?.Invoke();
        }

        /// <summary>
        /// Dismisses the specified view controller with the default blockRaycast setting (true).
        /// </summary>
        /// <param name="viewController">The view controller to dismiss.</param>
        public static void Dismiss(ViewController viewController)
        {
            Assert.IsFalse(viewController == null);

            Instance.StartCoroutine(DismissImpl(viewController, true));
        }

        /// <summary>
        /// Dismisses the specified view controller with the specified blockRaycast setting.
        /// </summary>
        /// <param name="viewController">The view controller to dismiss.</param>
        /// <param name="blockRaycast">Determines whether to block raycasts during the dismissal.</param>
        public static void Dismiss(ViewController viewController, bool blockRaycast = true)
        {
            Assert.IsFalse(viewController == null);

            Instance.StartCoroutine(DismissImpl(viewController, blockRaycast));
        }

        /// <summary>
        /// Dismisses the specified view controller of type T with the specified blockRaycast setting.
        /// </summary>
        /// <typeparam name="T2">Type of the view controller to dismiss.</typeparam>
        /// <param name="viewController">The view controller to dismiss.</param>
        /// <param name="blockRaycast">Determines whether to block raycasts during the dismissal.</param>
        public static void Dismiss<T2>(T2 viewController, bool blockRaycast = true) where T2 : ViewController
        {
            Assert.IsFalse(viewController == null);

            Instance.StartCoroutine(DismissImpl(viewController, blockRaycast));
        }

        /// <summary>
        /// Coroutine to handle the dismissal of a view controller.
        /// </summary>
        /// <param name="viewController">The view controller to dismiss.</param>
        /// <param name="blockRaycast">Determines whether to block raycasts during the dismissal.</param>
        private static IEnumerator DismissImpl(ViewController viewController, bool blockRaycast)
        {
            if (blockRaycast)
                SetEventSystemInactive(viewController.Transition.Disappear);

            viewController.OnWillDisappear();
            viewController.OnWillDisappearHandler?.Invoke();

            yield return new WaitForEndOfFrame();

            viewController.OnDismissTransition();

            yield return new WaitForSecondsRealtime(viewController.Transition.Disappear);

            viewController.OnDidDisappear();
            viewController.OnDidDisappearHandler?.Invoke();

            if(!viewController.Unconstrainted)
                Instance.Sections[viewController.Section].Remove(viewController);

            if (viewController != null)
                Destroy(viewController.gameObject);
        }

        /// <summary>
        /// Forces dismissal of the specified view controller without any transition animations.
        /// </summary>
        /// <param name="viewController">The view controller to dismiss forcibly.</param>
        public static void ForceDismiss(ViewController viewController)
        {
            Instance.Sections[viewController.Section].Remove(viewController);

            viewController.OnWillDisappear();
            viewController.OnWillDisappearHandler?.Invoke();

            viewController.OnDidDisappear();
            viewController.OnDidDisappearHandler?.Invoke();

            Destroy(viewController.gameObject);
        }

        /// <summary>
        /// Forces dismissal of all view controllers without any transition animations.
        /// </summary>
        public static void ForceDismissAll()
        {
            foreach (var layer in Instance.Sections)
            {
                foreach (var viewController in layer.Value)
                {
                    viewController.OnWillDisappear();
                    viewController.OnWillDisappearHandler?.Invoke();

                    viewController.OnDidDisappear();
                    viewController.OnDidDisappearHandler?.Invoke();

                    if (viewController != null)
                        Destroy(viewController.gameObject);
                }
            }

            foreach (var layer in Instance.Sections)
                layer.Value.Clear();
        }

        /// <summary>
        /// Temporarily disables the EventSystem to prevent interactions with the interface during animations.
        /// </summary>
        /// <param name="duration">The duration for which the EventSystem will be disabled.</param>
        private static void SetEventSystemInactive(float duration)
        {
            Assert.IsFalse(Instance._eventSystem == null);

            Instance._eventSystem.enabled = false;
            if (Instance._eventSystemCooldown < duration)
                Instance._eventSystemCooldown = duration;
        }

        /// <summary>
        /// Finds the nearest view controller plane distance in the specified section.
        /// </summary>
        /// <param name="section">The section in which the search should be performed.</param>
        /// <returns>The distance of the nearest view controller plane.</returns>
        public static float GetNearestCanvasPlaneDistance(string section)
        {
            if (section == null)
                return 100;

            var controllers = Instance.Sections;
            if (!controllers.ContainsKey(section))
                controllers.Add(section, new List<ViewController>());

            if (controllers[section].Count == 0)
                return 100;

            float distance = 100;
            for (int i = 0; i < controllers[section].Count; i++)
                if (controllers[section][i].Canvas.planeDistance < distance)
                    distance = controllers[section][i].Canvas.planeDistance;

            return distance - 0.5f;
        }

        public static List<T> FindViewControllersOfType<T>() where T : ViewController
        {
            List<T> result = new List<T>();

            foreach (var layer in Instance.Sections)
                for (int i = 0; i < layer.Value.Count; i++)
                    if (layer.Value[i].GetType() == typeof(T))
                        result.Add(layer.Value[i].GetComponent<T>());

            return result;
        }

        private void Reset()
        {
            OnValidate();
        }

        private void OnValidate()
        {
            if (name != "Storyboard")
                name = "Storyboard";

            if (_camera != null)
            {
                if (_audioListener == null)
                    _audioListener = _camera.GetComponent<AudioListener>();

                if (_camera.transform.parent != transform.parent)
                {
                    _camera.transform.SetParent(transform.parent, false);
                    _camera.transform.SetSiblingIndex(0);
                    _camera.name = "UICamera";
                }
            }

            if (_eventSystem == null)
            {
                EventSystem[] eventSystems = FindObjectsOfType<EventSystem>(true);
                if (eventSystems.Length > 0)
                    _eventSystem = eventSystems[0];
                else
                {
                    _eventSystem = new GameObject("EventSystem").AddComponent<EventSystem>();
                    _eventSystem.gameObject.AddComponent<StandaloneInputModule>();
                }
            }

            if(_eventSystem.transform.parent != transform.parent)
            {
                _eventSystem.transform.SetParent(transform.parent, false);
                if (_camera == null)
                    _eventSystem.transform.SetSiblingIndex(0);
                else
                    _eventSystem.transform.SetSiblingIndex(1);
            }

            if (_canvasScaler == null)
                _canvasScaler = GetComponent<CanvasScaler>();

            // Create layers.
            if (_layers.Under == null)
                _layers.Under = StoryboardUtils.CreateLayer("Under", transform);

            if (_layers.Base == null)
                _layers.Base = StoryboardUtils.CreateLayer("Base", transform);

            if (_layers.Extra == null)
                _layers.Extra = StoryboardUtils.CreateLayer("Extra", transform);

            if (_layers.Context == null)
                _layers.Context = StoryboardUtils.CreateLayer("Context", transform);

            if (_layers.Alert == null)
                _layers.Alert = StoryboardUtils.CreateLayer("Alert", transform);

            if (_layers.Over == null)
                _layers.Over = StoryboardUtils.CreateLayer("Over", transform);

            _layers.UpdateProperties(HideFlags.None);
        }

        public void SetMainCameraAsDefault()
        {
            _camera = Camera.main;
            _camera.orthographic = true;
            _camera.orthographicSize = 5;
        }

        public void NewCamera()
        {
            _camera = new GameObject("UICamera").AddComponent<Camera>();
            _camera.orthographic = true;
            _camera.orthographicSize = 5;
            _camera.transform.SetParent(transform.parent);
            _camera.transform.SetAsFirstSibling();
            _camera.transform.localScale = Vector3.one;
        }
    }
}