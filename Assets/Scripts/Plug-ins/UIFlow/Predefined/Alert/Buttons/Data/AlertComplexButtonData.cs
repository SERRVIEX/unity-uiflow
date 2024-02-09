namespace UIFlow
{
    using UnityEngine;

    using UIFlow.Predefined.Alert;

    public class AlertComplexButtonData : AlertButtonBase, ILabelData<AlertComplexButtonData>
    {
        public string Label { get; private set; }
        public Color LabelColor { get; private set; } = new Color32(255, 255, 255, 255);
        public IconData? LeftIcon { get; private set; }
        public IconData? RightIcon { get; private set; }

        // Methods

        public AlertComplexButtonData SetLabel(string value)
        {
            Label = value;
            return this;
        }

        public AlertComplexButtonData SetLabelColor(Color32 color)
        {
            LabelColor = color;
            return this;
        }

        public AlertComplexButtonData SetLeftIcon(Sprite icon, int width = 35, int height = 35)
        {
            LeftIcon = new IconData(icon, width, height);
            return this;
        }

        public AlertComplexButtonData SetRightIcon(Sprite icon, int width = 35, int height = 35)
        {
            RightIcon = new IconData(icon, width, height);
            return this;
        }
    }
}