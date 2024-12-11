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
            Canvas = FindObjectOfType<Canvas>(); // Find Canvas if not assigned
        }
    }

    public void Open()
    {
        if (currentPanel == null)
        {
            // Instantiate the panel and set the wallpaper image
            currentPanel = Instantiate(Panel, Canvas.transform);
            Image wallpaperImage = currentPanel.GetComponent<Image>();

            if (wallpaperImage != null)
            {
                wallpaperImage.sprite = GetComponent<Image>().sprite;

                // Convert the sprite's texture into a readable, uncompressed texture
                Texture2D texture = GetUncompressedTexture(wallpaperImage.sprite);

                // Encode the texture to JPG
                jpgData = texture.EncodeToJPG();
                SetWallpaper();

                // Clean up texture to free memory
                Destroy(texture);
            }
            else
            {
                Debug.LogError("Wallpaper Image component is missing!");
            }
        }
    } 

    private void SetWallpaper()
    {
            // Save the JPG to the persistent data path
            string filePath = Path.Combine(Application.persistentDataPath, "wallpaper.jpg");
            File.WriteAllBytes(filePath, jpgData);
            Debug.Log("Wallpaper saved as JPG!");

            // Provide a message to the user with the file path to set it manually
            Debug.Log("The wallpaper has been saved. You can set it manually at: " + filePath);

            // Set the wallpaper automatically
            // SetWallpaperDesktop(filePath);

            // Set the wallpaper automatically Android
            SetWallpaperAndroid(filePath);

            // Open the folder with the wallpaper (if supported in the Unity Editor)
            // OpenFolder(filePath);    

        
    }

    private void SetWallpaperAndroid(string filePath)
    {
        // Use the Android API to set the wallpaper
        try
        {

            // Use AndroidJavaObject to call Java's WallpaperManager
            using (AndroidJavaClass wallpaperManager = new AndroidJavaClass("android.app.WallpaperManager"))
            using (AndroidJavaObject context = GetContext())
            {
                AndroidJavaObject wallpaperManagerInstance = wallpaperManager.CallStatic<AndroidJavaObject>("getInstance", context);

                // Create a new File object pointing to the image path
                AndroidJavaObject file = new AndroidJavaObject("java.io.File", filePath);

                // Call the static fromFile() method on Uri class
                AndroidJavaClass uriClass = new AndroidJavaClass("android.net.Uri");
                AndroidJavaObject uri = uriClass.CallStatic<AndroidJavaObject>("fromFile", file);

                // Set the wallpaper using the WallpaperManager
                wallpaperManagerInstance.Call("setWallpaper", uri);
            }
        }
        catch (Exception ex)
        {
            Debug.LogError("Error setting wallpaper: " + ex.Message);
        }
    }

    AndroidJavaObject GetContext()
    {
        using (AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
        {
            return unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
        }
    }


    private void SetWallpaperDesktop(string filePath)
    {
        // Use the Windows API to set the wallpaper
        SystemParametersInfo(SPI_SETDESKWALLPAPER, 0, filePath, SPIF_UPDATEINIFILE | SPIF_SENDCHANGE);
    }

    private void OpenFolder(string filePath)
    {
        // Unity Editor-specific: Open the folder containing the wallpaper file (works in Editor)
#if UNITY_EDITOR
        UnityEditor.EditorUtility.RevealInFinder(filePath);  // This only works in the Unity Editor
#endif
    }

    Texture2D GetUncompressedTexture(Sprite sprite)
    {
        if (sprite == null || sprite.texture == null)
        {
            Debug.LogError("Sprite or its texture is null.");
            return null;
        }

        // Create a new uncompressed Texture2D with the same dimensions as the sprite
        Texture2D uncompressedTexture = new Texture2D((int)sprite.rect.width, (int)sprite.rect.height, TextureFormat.RGBA32, false);

        // Get the pixels from the original texture and copy them into the new uncompressed texture
        uncompressedTexture.SetPixels(sprite.texture.GetPixels(
            (int)sprite.textureRect.x,
            (int)sprite.textureRect.y,
            (int)sprite.textureRect.width,
            (int)sprite.textureRect.height));

        uncompressedTexture.Apply(); // Apply the changes

        return uncompressedTexture;
    }

}
