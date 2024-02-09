using UnityEngine;

public static class ScreenUtils
{
    public static float DPI
    {
        get
        {
#if UNITY_EDITOR
            return Screen.dpi;
#elif UNITY_ANDROID
            // Screen.dpi on some android device returns the wrong value, so the code below returns the real DPI.
            AndroidJavaClass activityClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject activity = activityClass.GetStatic<AndroidJavaObject>("currentActivity");

            AndroidJavaObject metrics = new AndroidJavaObject("android.util.DisplayMetrics");
            activity.Call<AndroidJavaObject>("getWindowManager").Call<AndroidJavaObject>("getDefaultDisplay").Call("getMetrics", metrics);

            return (metrics.Get<float>("xdpi") + metrics.Get<float>("ydpi")) * 0.5f;
#else
            return Screen.dpi;
#endif
        }
    }
}
