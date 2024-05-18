namespace UIFlow
{
    using UnityEngine;

    using DG.Tweening;

    public class PushPopoverTransition : Transition
    {
        private RectTransform _panel;

        // Constructors

        public PushPopoverTransition(ViewController current, RectTransform panel, float duration) : base(current, current.Previous, duration)
        {
            _panel = panel;
        }

        // Methods

        public override void Animate()
        {
            Current.CanvasGroup.alpha = 0;
            Current.CanvasGroup.DOFade(1, Duration);

            _panel.anchoredPosition = new Vector2(0, -_panel.rect.height);
            _panel.DOAnchorPosY(0, Duration);

            // From the center to the left.
            if (Previous != null)
            {
                var tween = Previous.RectTransform.DOScale(Vector3.one * 0.99f, Duration);
            }
        }
    }
}