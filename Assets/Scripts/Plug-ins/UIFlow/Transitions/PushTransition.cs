namespace UIFlow
{
    using UnityEngine;

    using DG.Tweening;

    public class PushTransition : Transition
    {
        public PushTransition(ViewController current, float duration) : base(current, current.Previous, duration)
        { }

        // Methods

        public override void Animate()
        {
            // From the right to the center.
            Current.RectTransform.anchoredPosition += new Vector2(Current.RectTransform.rect.width, 0);
            Current.RectTransform.DOAnchorPosX(0, Duration);

            // From the center to the left.
            if (Previous != null)
            {
                var tween = Previous.RectTransform.DOAnchorPosX(-Previous.RectTransform.rect.width, Duration);
                tween.onComplete += () =>
                {
                    Previous.gameObject.SetActive(false);
                };
            }
        }
    }
}