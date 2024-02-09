namespace UIFlow.Predefined.Alert
{
    using UnityEngine;

    public interface ILabelData<T> where T : AlertButtonBase
    {
        string Label { get; }
        Color LabelColor { get; }

        // Methods

        T SetLabel(string value);
        T SetLabelColor(Color32 color);
    }
}