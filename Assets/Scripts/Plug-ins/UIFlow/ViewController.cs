namespace UIFlow
{
    using UnityEngine;
    using UnityEngine.UI;
    using UnityEngine.Events;
    using UnityEngine.Assertions;

    using UIFlow.Core;

    [RequireComponent(typeof(Canvas))]
    [RequireComponent(typeof(CanvasScaler))]
    [RequireComponent(typeof(GraphicRaycaster))]
    [RequireComponent(typeof(CanvasGroup))]
    public abstract class ViewController : MonoBehaviour
    {
        public Layer Layer;
        public TransitionDuration Transition;

        public RectTransform RectTransform { get; private set; }
        public Canvas Canvas { get; private set; }
        [SerializeField] public CanvasGroup CanvasGroup { get; private set; }
        [field: SerializeField] public RectTransform Content { get; private set; }

        [HideInInspector] public UnityEvent OnWillAppearHandler = new UnityEvent();
        [HideInInspector] public UnityEvent OnDidAppearHandler = new UnityEvent();
        [HideInInspector] public UnityEvent OnWillDisappearHandler = new UnityEvent();
        [HideInInspector] public UnityEvent OnDidDisappearHandler = new UnityEvent();

        // Methods

        protected virtual void Awake()
        {
            RectTransform = GetComponent<RectTransform>();

            Canvas = GetComponent<Canvas>();
            Canvas.renderMode = RenderMode.ScreenSpaceCamera;

            CanvasGroup = GetComponent<CanvasGroup>();

            Content = transform.Find("Content").GetComponent<RectTransform>();
        }

        protected virtual void Start()
        {
            float planeDistance = Storyboard.FindNearestDistance(Layer);
            Canvas.planeDistance = planeDistance;
        }

        public virtual void OnWillAppear() { }

        public virtual void OnDidAppear() { }

        public virtual void OnDidDisappear() { }

        public virtual void OnWillDisappear() { }

        public virtual void OnAppearTransition() { }

        public virtual void OnDisappearTransition() { }

        public virtual void Dismiss() => Storyboard.Dismiss(this, false);
        public virtual void Dismiss(bool blockRaycast = true) => Storyboard.Dismiss(this, false);

        protected virtual void OnValidate()
        {
            name = GetType().Name;

            RectTransform rectTransform = GetComponent<RectTransform>();
            Assert.IsTrue(rectTransform != null);

            rectTransform.SetPivot(Pivot.MiddleCenter);
            rectTransform.SetAnchor(Anchor.Stretch);
            rectTransform.SetOffset(0, 0, 0, 0);

            if(Content == null)
            {
                RectTransform content = transform.Find("Content") as RectTransform;
                if (content != null)
                    Content = content;
                
                else
                {
                    Content = new GameObject("Content").AddComponent<RectTransform>();
                    Content.SetParent(transform, false);
                    Content.gameObject.layer = 5;
                }

                Content.localPosition  = Vector3.zero;
                Content.localRotation = Quaternion.identity;
                Content.localScale = Vector3.one;
                Content.SetAnchor(Anchor.Stretch);
                Content.SetOffset(0, 0, 0, 0);
            }

            Content.name = "Content";
        }
    }
}