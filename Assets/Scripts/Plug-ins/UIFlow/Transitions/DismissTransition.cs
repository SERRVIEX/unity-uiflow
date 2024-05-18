namespace UIFlow
{
    using DG.Tweening;

    public class DismissTransition : Transition
    {
        public DismissTransition(ViewController current, float duration) : base(current, current.Previous, duration)
        { }

        // Methods

        public override void Animate()
        {
            // From the center to the right.
            Current.RectTransform.DOAnchorPosX(Current.RectTransform.rect.width, Duration);
            // From the left to the center.
            if (Previous != null)
            {
                Previous.gameObject.SetActive(true);
                Previous.RectTransform.DOAnchorPosX(0, Duration);
            }
        }
    }
}