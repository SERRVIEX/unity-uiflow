namespace UIFlow
{
    using UnityEngine;
    using UnityEngine.Events;

    public abstract class AlertButtonBase
    {
        public Color BackgroundColor { get; private set; } = new Color32(128, 128, 128, 255);
        public UnityAction OnClick { get; private set; }

        // Methods

        public AlertButtonBase SetBackgroundColor(Color backgroundColor)
        {
            BackgroundColor = backgroundColor;
            return this;
        }

        public AlertButtonBase SetOnClick(UnityAction onClick)
        {
            OnClick = onClick;
            return this;
        }
    }
}