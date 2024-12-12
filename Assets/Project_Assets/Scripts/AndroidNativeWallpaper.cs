using UnityEngine;

public static class AndroidNativeWallpaper
{
    public static void SetWallpaper(string filePath)
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        using (AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
        {
            using (AndroidJavaObject currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity"))
            {
                using (AndroidJavaObject wallpaperManager = new AndroidJavaClass("android.app.WallpaperManager").CallStatic<AndroidJavaObject>("getInstance", currentActivity))
                {
                    using (AndroidJavaObject fileInputStream = new AndroidJavaObject("java.io.FileInputStream", filePath))
                    {
                        wallpaperManager.Call("setStream", fileInputStream);
                    }
                }
            }
        }
#else
        Debug.Log("SetWallpaper is only available on Android.");
#endif
    }
}
