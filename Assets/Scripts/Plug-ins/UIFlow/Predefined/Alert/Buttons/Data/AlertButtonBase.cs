namespace UIFlow
{
    using UnityEngine;
    using UnityEngine.Events;

    public abstract class AlertButtonBase
    {
        public Color BackgroundColor { get; private set; } = Color.white;
        public UnityAction OnClick { get; private set; }

        // Constructors

        public AlertButtonBase()
        {
            //BackgroundColor = Palette.GetColor("main_shade_3", "value");
        }


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