using System;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Runtime.InteropServices;

public class WallpaperDownload : MonoBehaviour
{
    public GameObject Panel; // Prefab for the wallpaper detail panel
    public Canvas Canvas; // Reference to the main canvas
    private GameObject currentPanel; // Tracks the instantiated panel
    private byte[] jpgData;

    // Define Windows API functions for setting wallpaper
    [DllImport("user32.dll", CharSet = CharSet.Auto)]
    public static extern int SystemParametersInfo(int uAction, int uParam, string lpvParam, int fuWinIni);

    const int SPI_SETDESKWALLPAPER = 20;
    const int SPIF_UPDATEINIFILE = 0x01;
    const int SPIF_SENDCHANGE = 0x02;

    private void Awake()
    {
        if (Canvas == null)
        {
            Canvas = FindObjectOfType<Canvas>();
            if (Canvas == null)
            {
                Debug.LogError("Canvas not found in the scene.");
                return;
            }
        }
    }

    public void Open()
    {
        if (currentPanel == null)
        {
            currentPanel = Instantiate(Panel, Canvas.transform);
        }

        SetupWallpaperImage();
    }

     private void SetupWallpaperImage()
     {
        if (currentPanel == null) return;

        Image wallpaperImage = currentPanel.GetComponent<Image>();

        if (wallpaperImage == null)
        {
            Debug.LogError("Wallpaper Image component is missing!");
            return;
        }

        wallpaperImage.sprite = GetComponent<Image>().sprite;

        Texture2D texture = GetUncompressedTexture(wallpaperImage.sprite);
        if (texture == null)
        {
            Debug.LogError("Failed to create uncompressed texture.");
            return;
        }

        jpgData = texture.EncodeToJPG();
        SetWallpaper();
        Destroy(texture); // Clean up memory
     }


    private void SetWallpaper()
    {
        if (jpgData == null || jpgData.Length == 0)
        {
            Debug.LogError("No wallpaper data available to set.");
            return;
        }

        string filePath = SaveWallpaperToFile();
        if (string.IsNullOrEmpty(filePath)) return;

        // Provide instructions for manual setting
        Debug.Log($"Wallpaper saved at: {filePath}. You can set it manually.");

        #if UNITY_ANDROID
                SetWallpaperAndroid(filePath);
        #elif UNITY_STANDALONE_WIN
                SetWallpaperDesktop(filePath);
        #endif

    }

    private string SaveWallpaperToFile()
    {
        try
        {
            string filePath = Path.Combine(Application.persistentDataPath, "wallpaper.jpg");
            File.WriteAllBytes(filePath, jpgData);
            Debug.Log("Wallpaper saved successfully!");
            return filePath;
        }
        catch (Exception ex)
        {
            Debug.LogError("Failed to save wallpaper: " + ex.Message);
            return null;
        }
    }

    private void SetWallpaperAndroid(string filePath)
    {
        try
        {
            AndroidJavaObject context = GetContext();
            if (context == null)
            {
                Debug.LogError("Failed to get context.");
                return;
            }

            using (AndroidJavaClass wallpaperManagerClass = new AndroidJavaClass("android.app.WallpaperManager"))
            {
                AndroidJavaObject wallpaperManager = wallpaperManagerClass.CallStatic<AndroidJavaObject>("getInstance", context);
                if (wallpaperManager == null)
                {
                    Debug.LogError("WallpaperManager initialization failed.");
                    return;
                }

                // Handle file and URI
                using (AndroidJavaClass uriClass = new AndroidJavaClass("android.net.Uri"))
                using (AndroidJavaObject file = new AndroidJavaObject("java.io.File", filePath))
                {
                    AndroidJavaObject uri = uriClass.CallStatic<AndroidJavaObject>("fromFile", file);
                    if (uri == null)
                    {
                        Debug.LogError("Uri creation failed.");
                        return;
                    }

                    // Check Android version to decide the method to call
                    int sdkVersion = GetAndroidSdkVersion();
                    if (sdkVersion >= 24) // Android 7.0 (API level 24) and higher
                    {
                        // Use setBitmap for newer Android versions
                        using (AndroidJavaObject bitmap = ConvertTextureToBitmap(filePath))
                        {
                            wallpaperManager.Call("setBitmap", bitmap);
                        }
                    }
                    else
                    {
                        // Use setStream for older Android versions
                        wallpaperManager.Call("setStream", uri);
                    }

                    Debug.Log("Wallpaper set successfully!");
                }
            }
        }
        catch (Exception ex)
        {
            Debug.LogError("Error setting wallpaper: " + ex.Message);
        }
    }

    // Helper method to get the Android SDK version
    private int GetAndroidSdkVersion()
    {
        try
        {
            using (AndroidJavaClass versionClass = new AndroidJavaClass("android.os.Build$VERSION"))
            {
                return versionClass.GetStatic<int>("SDK_INT");
            }
        }
        catch (Exception ex)
        {
            Debug.LogError("Error getting Android SDK version: " + ex.Message);
            return 0;
        }
    }

    // Converts file path to a Bitmap (only for setBitmap use)
    private AndroidJavaObject ConvertTextureToBitmap(string filePath)
    {
        using (AndroidJavaClass bitmapFactory = new AndroidJavaClass("android.graphics.BitmapFactory"))
        using (AndroidJavaObject file = new AndroidJavaObject("java.io.File", filePath))
        using (AndroidJavaObject fileInputStream = new AndroidJavaObject("java.io.FileInputStream", file))
        {
            return bitmapFactory.CallStatic<AndroidJavaObject>("decodeStream", fileInputStream);
        }
    }


    private AndroidJavaObject GetContext()
    {
        try
        {
            using (AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
            {
                AndroidJavaObject activity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
                if (activity == null)
                {
                    Debug.LogError("UnityPlayer.currentActivity is null.");
                    return null;
                }

                Debug.Log("Successfully retrieved currentActivity.");
                return activity;
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Error accessing currentActivity: " + e.Message);
            return null;
        }
    }


    private AndroidJavaObject ConvertTextureToBitmap(Texture2D texture)
    {
        byte[] textureData = texture.EncodeToPNG();

        using (AndroidJavaClass bitmapFactory = new AndroidJavaClass("android.graphics.BitmapFactory"))
        using (AndroidJavaObject bitmap = bitmapFactory.CallStatic<AndroidJavaObject>("decodeByteArray", textureData, 0, textureData.Length))
        {
            return bitmap;
        }
    }


    private void SetWallpaperDesktop(string filePath)
    {
        try
        {
            SystemParametersInfo(SPI_SETDESKWALLPAPER, 0, filePath, SPIF_UPDATEINIFILE | SPIF_SENDCHANGE);
            Debug.Log("Wallpaper set successfully on Windows.");
        }
        catch (Exception ex)
        {
            Debug.LogError("Error setting wallpaper on Windows: " + ex.Message);
        }
    }

    private void OpenFolder(string filePath)
    {
        // Unity Editor-specific: Open the folder containing the wallpaper file (works in Editor)
#if UNITY_EDITOR
        UnityEditor.EditorUtility.RevealInFinder(filePath);  // This only works in the Unity Editor
#endif
    }

    private Texture2D GetUncompressedTexture(Sprite sprite)
    {
        if (sprite == null || sprite.texture == null)
        {
            Debug.LogError("Sprite or its texture is null.");
            return null;
        }

        Texture2D uncompressedTexture = new Texture2D((int)sprite.rect.width, (int)sprite.rect.height, TextureFormat.RGBA32, false);

        try
        {
            Color[] pixels = sprite.texture.GetPixels(
                (int)sprite.textureRect.x,
                (int)sprite.textureRect.y,
                (int)sprite.textureRect.width,
                (int)sprite.textureRect.height);
            uncompressedTexture.SetPixels(pixels);
            uncompressedTexture.Apply();
        }
        catch (Exception ex)
        {
            Debug.LogError("Error processing texture: " + ex.Message);
            Destroy(uncompressedTexture);
            return null;
        }

        return uncompressedTexture;
    }

}
