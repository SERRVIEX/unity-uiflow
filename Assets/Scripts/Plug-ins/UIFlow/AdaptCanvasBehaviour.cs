namespace UIFlow
{
    using UnityEngine;
    using UnityEngine.UI;

    using UIFlow.Utils;

    public class AdaptCanvasBehaviour : MonoBehaviour
    {
        private void Start()
        {
            CanvasScaler canvasScaler = GetComponent<CanvasScaler>();
            StoryboardUtils.AdaptCanvasScaler(canvasScaler);
        }
    }
}