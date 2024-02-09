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

        [SerializeField] private ViewController _initialViewController;
        [SerializeField] private ViewController[] _cachedViewControllers;

        private Dictionary<Layer, List<ViewController>> _controllers;
        private float _eventSystemCooldown;

        // Methods

        private void Awake()
        {
            if(Instance != null)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;

            Assert.IsNotNull(_camera);
            Assert.IsNotNull(_eventSystem);

            Assert.IsNotNull(_canvasScaler);

            Assert.IsNotNull(_layers.Under);
            Assert.IsNotNull(_layers.Base);
            Assert.IsNotNull(_layers.Extra);
            Assert.IsNotNull(_layers.Context);
            Assert.IsNotNull(_layers.Alert);
            Assert.IsNotNull(_layers.Over);

            AdaptCanvas();

            _controllers = new Dictionary<Layer, List<ViewController>>
            {
                { Layer.Under, new List<ViewController>() },
                { Layer.Base, new List<ViewController>() },
                { Layer.Extra, new List<ViewController>() },
                { Layer.Context, new List<ViewController>() },
                { Layer.Alert, new List<ViewController>() },
                { Layer.Over, new List<ViewController>() }
            };

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
        /// Return the current screen orientation that also work in the editor without simulator.
        /// </summary>
        private ScreenOrientation GetScreenOrientation()
        {
#if UNITY_EDITOR
            return Screen.height > Screen.width ? ScreenOrientation.Portrait : ScreenOrientation.LandscapeLeft;
#else
            return Screen.orientation;
#endif
        }

        public static Canvas GetLayer(Layer layer)
        {
            switch (layer)
            {
                case Layer.Under: return Instance._layers.Under;
                case Layer.Base: return Instance._layers.Base;
                case Layer.Extra: return Instance._layers.Extra;
                case Layer.Context: return Instance._layers.Context;
                case Layer.Alert: return Instance._layers.Alert;
                case Layer.Over: return Instance._layers.Over;
                default: return Instance._layers.Under;
            }
        }

        public static void Present(ViewController viewController)
        {

            Assert.IsFalse(viewController == null);

            ViewController obj = Instantiate(viewController, Instance._layers.GetCanvas(viewController.Layer).transform);
            obj.Canvas.worldCamera = Instance._camera;

            Instance.StartCoroutine(PresentImpl(obj, true));
        }

        public static void Present(ViewController viewController, bool blockRaycast = true)
        {
            Assert.IsFalse(viewController == null);

            ViewController obj = Instantiate(viewController, Instance._layers.GetCanvas(viewController.Layer).transform);
            obj.Canvas.worldCamera = Instance._camera;

            Instance.StartCoroutine(PresentImpl(obj, blockRaycast));
        }

        public static T2 Present<T2>(T2 viewController, bool blockRaycast = true) where T2 : ViewController
        {
            Assert.IsFalse(viewController == null);

            ViewController obj = Instantiate(viewController, Instance._layers.GetCanvas(viewController.Layer).transform);
            obj.Canvas.worldCamera = Instance._camera;

            Instance.StartCoroutine(PresentImpl(obj, blockRaycast));

            return (T2)obj;
        }

        public static T2 Present<T2>(bool blockRaycast = true) where T2 : ViewController
        {
            ViewController viewController = GetCachedViewController<T2>();
            Assert.IsFalse(viewController == null);

            ViewController obj = Instantiate(viewController, Instance._layers.GetCanvas(viewController.Layer).transform);
            obj.Canvas.worldCamera = Instance._camera;

            Instance.StartCoroutine(PresentImpl(obj, blockRaycast));

            return (T2)obj;
        }

        private static IEnumerator PresentImpl(ViewController obj, bool blockRaycast)
        {
            if (blockRaycast)
                SetEventSystemInactive(obj.Transition.Appear);
            Instance._controllers[obj.Layer].Add(obj);

            obj.OnWillAppear();
            obj.OnWillAppearHandler?.Invoke();

            obj.OnAppearTransition();

            yield return new WaitForSecondsRealtime(obj.Transition.Appear);

            obj.OnDidAppear();
            obj.OnDidAppearHandler?.Invoke();
        }

        public static void Dismiss(ViewController viewController)
        {
            Assert.IsFalse(viewController == null);

            Instance.StartCoroutine(DismissImpl(viewController, true));
        }

        public static void Dismiss(ViewController viewController, bool blockRaycast = true)
        {
            Assert.IsFalse(viewController == null);

            Instance.StartCoroutine(DismissImpl(viewController, blockRaycast));
        }

        public static void Dismiss<T2>(T2 viewController, bool blockRaycast = true) where T2 : ViewController
        {
            Assert.IsFalse(viewController == null);

            Instance.StartCoroutine(DismissImpl(viewController, blockRaycast));
        }

        private static IEnumerator DismissImpl(ViewController viewController, bool blockRaycast)
        {
            if (blockRaycast)
                SetEventSystemInactive(viewController.Transition.Disappear);
            Instance._controllers[viewController.Layer].Remove(viewController);

            viewController.OnWillDisappear();
            viewController.OnWillDisappearHandler?.Invoke();

            viewController.OnDisappearTransition();

            yield return new WaitForSecondsRealtime(viewController.Transition.Disappear);

            viewController.OnDidDisappear();
            viewController.OnDidDisappearHandler?.Invoke();

            if (viewController != null)
                Destroy(viewController.gameObject);
        }

        public static void ForceDismissAll()
        {
            foreach (var layer in Instance._controllers)
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

            foreach (var layer in Instance._controllers)
                layer.Value.Clear();
        }

        /// <summary>
        /// Don't allow interactions with thee interface when animation is doing.
        /// </summary>
        /// <param name="duration">How many will be inactive.</param>
        private static void SetEventSystemInactive(float duration)
        {
            Assert.IsFalse(Instance._eventSystem == null);

            Instance._eventSystem.enabled = false;
            if (Instance._eventSystemCooldown < duration)
                Instance._eventSystemCooldown = duration;
        }

        /// <summary>
        /// Find the nearest view controller plane distance.
        /// </summary>
        /// <param name="layer">In which layer the search should be performed.</param>
        public static float FindNearestDistance(Layer layer)
        {
            if (Instance._controllers[layer].Count == 0)
                return 100;

            float distance = 100;
            for (int i = 0; i < Instance._controllers[layer].Count; i++)
                if (Instance._controllers[layer][i].Canvas.planeDistance < distance)
                    distance = Instance._controllers[layer][i].Canvas.planeDistance;

            return distance - 0.5f;
        }

        public static List<T2> FindViewControllersOfType<T2>() where T2 : ViewController
        {
            List<T2> result = new List<T2>();

            foreach (var layer in Instance._controllers)
                for (int i = 0; i < layer.Value.Count; i++)
                    if (layer.Value[i].GetType() == typeof(T2))
                        result.Add(layer.Value[i].GetComponent<T2>());

            return result;
        }

        public static T2 GetCachedViewController<T2>() where T2 : ViewController
        {
            for (int i = 0; i < Instance._cachedViewControllers.Length; i++)
                if (Instance._cachedViewControllers[i].GetType() == typeof(T2))
                    return (T2)Instance._cachedViewControllers[i];

            return null;
        }

        private void Reset()
        {
            OnValidate();

            List<ViewController> cachedViewControllers = new List<ViewController>();

            var alerts = Resources.FindObjectsOfTypeAll<AlertViewController>();
            if (alerts.Length > 0)
                cachedViewControllers.Add(alerts[0]);

            var commands = Resources.FindObjectsOfTypeAll<CommandViewController>();
            if (commands.Length > 0)
                cachedViewControllers.Add(commands[0]);

            var loadings = Resources.FindObjectsOfTypeAll<LoadingViewController>();
            if (loadings.Length > 0)
                cachedViewControllers.Add(loadings[0]);

            var toasts = Resources.FindObjectsOfTypeAll<ToastViewController>();
            if (toasts.Length > 0)
                cachedViewControllers.Add(toasts[0]);

            _cachedViewControllers = cachedViewControllers.ToArray();
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