namespace UIFlow.Utils
{
    using UnityEngine;
    using UnityEngine.UI;

    public static class StoryboardUtils
    {
        public static Canvas CreateLayer(string title, Transform parent)
        {
            GameObject obj = new GameObject(title);
            obj.transform.SetParent(parent);
            obj.layer = 5;

            RectTransform rectTransform = obj.AddComponent<RectTransform>();
            rectTransform.localPosition = Vector3.zero;
            rectTransform.localRotation = Quaternion.identity;
            rectTransform.localScale = Vector3.one;

            rectTransform.SetAnchor(Anchor.Stretch);
            rectTransform.SetPivot(Pivot.MiddleCenter);

            rectTransform.SetOffset(0, 0, 0, 0);

            Canvas canvas = obj.AddComponent<Canvas>();
            canvas.overrideSorting = true;

            obj.AddComponent<CanvasScaler>();
            obj.AddComponent<GraphicRaycaster>();

            return canvas;
        }

        /// <summary>
        /// Adapt canvas scaler to target screen.
        /// </summary>
        public static void AdaptCanvasScaler(CanvasScaler canvasScaler)
        {
            if (SystemUtils.Device == SystemUtils.DeviceType.Phone)
            {
                canvasScaler.matchWidthOrHeight = 0;
              
                if (GetScreenOrientation() == ScreenOrientation.Portrait)
                {
                    return;
                    if(Screen.width < 1080)
                        canvasScaler.referenceResolution = new Vector2(1080f * (1080f / Screen.width), canvasScaler.referenceResolution.y);
                    
                    else
                        canvasScaler.referenceResolution = new Vector2(1080 * (Screen.height / 1920f), canvasScaler.referenceResolution.y);
                }
            }
            else
            {
                canvasScaler.matchWidthOrHeight = 0;

                if (GetScreenOrientation() == ScreenOrientation.Portrait)
                    canvasScaler.referenceResolution = new Vector2(Screen.height, 1920);
                else
                    canvasScaler.referenceResolution = new Vector2(1920, 1080);
            }
        }

        /// <summary>
        /// Return the current screen orientation that also work in the editor without simulator.
        /// </summary>
        private static ScreenOrientation GetScreenOrientation()
        {
#if UNITY_EDITOR
            return Screen.height > Screen.width ? ScreenOrientation.Portrait : ScreenOrientation.LandscapeLeft;
#else
            return Screen.orientation;
#endif
        }
    }
}