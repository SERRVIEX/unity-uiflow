namespace UIFlow
{
    using UnityEngine;
    using UnityEngine.UI;

    using TMPro;

    public class AlertComplexButtonView : AlertButtonViewBase
    {
        [SerializeField] protected TextMeshProUGUI Label;
        [SerializeField] protected Image LeftIcon;
        [SerializeField] protected Image RightIcon;

        // Methods

        public override void SetData(AlertButtonBase data)
        {
            base.SetData(data);

            AlertComplexButtonData complexData = data as AlertComplexButtonData;

            Label.text = complexData.Label;
            Label.color = complexData.LabelColor;

            if (complexData.LeftIcon != null)
            {
                LeftIcon.transform.parent.gameObject.SetActive(true);
                LeftIcon.sprite = complexData.LeftIcon.Value.Icon;
                LeftIcon.rectTransform.SetSize(complexData.LeftIcon.Value.Width, complexData.LeftIcon.Value.Height);
            }
            else
                LeftIcon.transform.parent.gameObject.SetActive(false);

            if (complexData.RightIcon != null)
            {
                RightIcon.transform.parent.gameObject.SetActive(true);
                RightIcon.sprite = complexData.RightIcon.Value.Icon;
                RightIcon.rectTransform.SetSize(complexData.RightIcon.Value.Width, complexData.RightIcon.Value.Height);
            }
            else
                RightIcon.transform.parent.gameObject.SetActive(false);
        }

        public override void OnValidate()
        {
            base.OnValidate();

            if (Label == null)
                Label = transform.Find("Label").GetComponent<TextMeshProUGUI>();

            if (LeftIcon == null)
                LeftIcon = transform.Find("LeftIconBox/Icon").GetComponent<Image>();

            if (RightIcon == null)
                RightIcon = transform.Find("RightIconBox/Icon").GetComponent<Image>();
        }
    }
}