namespace UIFlow
{
    using UnityEngine;

    using UIFlow.Predefined.Alert;

    public class AlertDefaultButtonData : AlertButtonBase, ILabelData<AlertDefaultButtonData>
    {
        public string Label { get; private set; }
        public Color LabelColor { get; private set; } = new Color32(255, 255, 255, 255);
        public IconData? Icon { get; private set; }

        // Methods

        public AlertDefaultButtonData SetLabel(string value)
        {
            Label = value;
            return this;
        }

        public AlertDefaultButtonData SetLabelColor(Color32 color)
        {
            LabelColor = color;
            return this;
        }

        public AlertDefaultButtonData SetIcon(Sprite icon, int width = 35, int height = 35)
        {
            Icon = new IconData(icon, width, height);
            return this;
        }
    }
}