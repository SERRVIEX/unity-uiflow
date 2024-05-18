namespace UIFlow
{
    using UnityEngine;

    using TMPro;
    using UnityEngine.UI;

    public class Header : MonoBehaviour
    {
        [field: SerializeField] protected TextMeshProUGUI Title { get; private set; }

        [field: SerializeField, Header("Scroll Title")] protected ScrollRect ScrollRect { get; private set; }
        [field: SerializeField] protected TextMeshProUGUI ScrollTitle { get; private set; }


        // Methods

        public virtual void SetTitle(string value)
        {
            Title.text = value;
            ScrollTitle.text = value;
        }

        protected virtual void Update()
        {
            if(ScrollRect != null && ScrollTitle != null)
            {
                var rectTransform = ScrollTitle.rectTransform;
                float t = (ScrollRect.content.anchoredPosition.y - rectTransform.rect.height / 1.25f) / rectTransform.rect.height;
                if (t <= 0)
                    t = 0;

                Color color = Title.color;
                color.a = Mathf.Lerp(0, 1, t);
                Title.color = color;
            }
        }
    }
}