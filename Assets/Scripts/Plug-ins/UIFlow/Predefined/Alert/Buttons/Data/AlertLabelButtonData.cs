namespace UIFlow
{
    using UIFlow.Predefined.Alert;
    using UnityEngine;

    public class AlertLabelButtonData : AlertButtonBase, ILabelData<AlertLabelButtonData>
    {
        public string Label { get; private set; }
        public Color LabelColor { get; private set; } = new Color32(25, 25, 25, 255);

        // Constructors

        public AlertLabelButtonData()
        {
            //LabelColor = Palette.GetColor("text_shade_1", "value");
        }

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