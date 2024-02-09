namespace UIFlow.Predefined.Alert
{
    using UnityEngine;
    using UnityEngine.UI;

    public class AlertIconButtonView : AlertButtonViewBase
    {
        [SerializeField] protected Image Icon;

        // Methods

        public override void SetData(AlertButtonBase data)
        {
            base.SetData(data);

            AlertIconButtonData iconButton = data as AlertIconButtonData;
            if (iconButton.Icon != null)
            {
                Icon.transform.parent.gameObject.SetActive(true);
                Icon.sprite = iconButton.Icon.Value.Icon;
                Icon.rectTransform.SetSize(iconButton.Icon.Value.Width, iconButton.Icon.Value.Height);
            }
            else
                Icon.transform.parent.gameObject.SetActive(false);
        }

        public override void OnValidate()
        {
            base.OnValidate();

            if (Icon == null)
                Icon = transform.Find("IconBox/Icon").GetComponent<Image>();
        }
    }
}