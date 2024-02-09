using UnityEngine;

public static class SystemUtils
{
    public enum DeviceType
    {
        Unknown = 0,
        Console = 1,
        Desktop = 2,
        Tablet = 3,
        Phone = 4,
    }

    public static DeviceType Device
    {
        get
        {
#if UNITY_EDITOR
            return DeviceType.Phone;
#elif UNITY_IOS
            if (UnityEngine.iOS.Device.generation.ToString().Contains("iPad"))
                return DeviceType.Tablet;

            return DeviceType.Phone;
#elif UNITY_ANDROID
            float aspectRatio = (float)Mathf.Max(Screen.width, Screen.height) / Mathf.Min(Screen.width, Screen.height);
            if (aspectRatio < 2f)
            {
                float inches = Mathf.Sqrt(Screen.width * Screen.width + Screen.height * Screen.height) / ScreenUtils.DPI;
                if (inches >= 6.5f)
                    return DeviceType.Tablet;
            }

            return DeviceType.Phone;
#else
            switch (SystemInfo.deviceType)
            {
                case UnityEngine.DeviceType.Console: return DeviceType.Console;
                case UnityEngine.DeviceType.Desktop: return DeviceType.Desktop;
            }

            return DeviceType.Unknown;
#endif
        }
    }
}
