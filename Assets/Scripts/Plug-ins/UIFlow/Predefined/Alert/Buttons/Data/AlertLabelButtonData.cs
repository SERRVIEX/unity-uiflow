namespace UIFlow
{
    using UIFlow.Predefined.Alert;
    using UnityEngine;

    public class AlertLabelButtonData : AlertButtonBase, ILabelData<AlertLabelButtonData>
    {
        public string Label { get; private set; }
        public Color LabelColor { get; private set; } = new Color32(255, 255, 255, 255);

        // Methods

        public AlertLabelButtonData SetLabel(string value)
        {
            Label = value;
            return this;
        }

        public AlertLabelButtonData SetLabelColor(Color32 color)
        {
            LabelColor = color;
            return this;
        }
    }
}