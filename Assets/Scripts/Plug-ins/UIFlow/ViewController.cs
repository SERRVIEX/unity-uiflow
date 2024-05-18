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
        public bool Initialized { get; private set; }
        public string Section { get; private set; }
        public Layer Layer => _layer;
        [SerializeField] private Layer _layer;

        public TransitionDuration Transition => _transition;
        [SerializeField] private TransitionDuration _transition;

        public bool Unconstrainted => _unconstrainted;
        [SerializeField] private bool _unconstrainted;

        public ViewController Previous => Storyboard.GetPreviousViewController(this);

        public RectTransform RectTransform { get; private set; }
        public Canvas Canvas { get; private set; }
        public CanvasGroup CanvasGroup { get; private set; }
        public RectTransform Content { get; private set; }

        [HideInInspector] public UnityEvent OnWillAppearHandler = new UnityEvent();
        [HideInInspector] public UnityEvent OnDidAppearHandler = new UnityEvent();
        [HideInInspector] public UnityEvent OnWillDisappearHandler = new UnityEvent();
        [HideInInspector] public UnityEvent OnDidDisappearHandler = new UnityEvent();

        // Methods

        public void InitializeController(string section)
        {
            RectTransform = GetComponent<RectTransform>();

            Canvas = GetComponent<Canvas>();
            Canvas.renderMode = RenderMode.ScreenSpaceCamera;

            float planeDistance = Storyboard.GetNearestCanvasPlaneDistance(Section);
            Canvas.planeDistance = planeDistance;

            CanvasGroup = GetComponent<CanvasGroup>();

            Content = transform.Find("Content").GetComponent<RectTransform>();

            Initialized = true;
            Section = section;
        }

        public virtual void OnWillAppear() { }

        public virtual void OnDidAppear() { }

        public virtual void OnDidDisappear() { }

        public virtual void OnWillDisappear() { }

        public virtual void OnPresentTransition() { }

        public virtual void OnDismissTransition() { }

        public virtual void Dismiss() => Storyboard.Dismiss(this, false);
        public virtual void Dismiss(bool blockRaycast = true) => Storyboard.Dismiss(this, false);
        public virtual void ForceDismiss() => Storyboard.ForceDismiss(this);

        protected virtual void OnValidate()
        {
            name = GetType().Name;

            var rectTransform = GetComponent<RectTransform>();
            Assert.IsTrue(rectTransform != null);

            rectTransform.SetPivot(Pivot.MiddleCenter);
            rectTransform.SetAnchor(Anchor.Stretch);
            rectTransform.SetOffset(0, 0, 0, 0);

            if(Content == null)
            {
                var content = transform.Find("Content") as RectTransform;
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