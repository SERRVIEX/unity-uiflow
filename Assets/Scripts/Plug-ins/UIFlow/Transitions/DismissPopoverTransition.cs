namespace UIFlow
{
    using UnityEngine;

    using DG.Tweening;

    public class DismissPopoverTransition : Transition
    {
        private RectTransform _panel;

        public DismissPopoverTransition(ViewController current, RectTransform panel, float duration) : base(current, current.Previous, duration)
        {
            _panel = panel;
        }

        // Methods

        public override void Animate()
        {
            Current.CanvasGroup.DOFade(0, Duration);
            _panel.DOAnchorPosY(-_panel.rect.height, Duration);

            // From the left to the center.
            if (Previous != null)
                Previous.RectTransform.DOScale(1, Duration);
        }
    }
}