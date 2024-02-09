namespace UIFlow
{
    using UnityEngine;

    using UIFlow.Predefined.Alert;

    public class AlertIconButtonData : AlertButtonBase
    {
        public IconData? Icon { get; private set; }

        // Methods

        public AlertIconButtonData SetIcon(Sprite icon, int width = 35, int height = 35)
        {
            Icon = new IconData(icon, width, height);
            return this;
        }
    }
}