using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Android;
using System.IO;

public class WallpaperManager : MonoBehaviour
{
    public GameObject fullScreenPanel; // Panel for full-screen image
    public Image fullScreenImage; // Full-screen image component
    public Button setWallpaperButton; // Button to set wallpaper
    public Sprite[] thumbnailSprites; // Array of thumbnail sprites (loaded locally or dynamically)

    public Transform thumbnailParent; // Parent object for thumbnails
    public GameObject thumbnailPrefab; // Prefab for thumbnail buttons

    void Start()
    {
        // Populate thumbnails
        foreach (Sprite sprite in thumbnailSprites)
        {
            GameObject thumbnail = Instantiate(thumbnailPrefab, thumbnailParent);
            thumbnail.GetComponent<Image>().sprite = sprite;
            thumbnail.GetComponent<Button>().onClick.AddListener(() => OpenFullImage(sprite));
        }

        // Assign wallpaper setting functionality
        setWallpaperButton.onClick.AddListener(SetWallpaper);
    }

    // Opens the selected image in full-screen mode
    void OpenFullImage(Sprite sprite)
    {
        fullScreenPanel.SetActive(true);
        fullScreenImage.sprite = sprite;
    }

    // Closes the full-screen image
    public void CloseFullImage()
    {
        fullScreenPanel.SetActive(false);
    }

    // Sets the current full-screen image as the phone wallpaper
    void SetWallpaper()
    {
        Texture2D texture = fullScreenImage.sprite.texture;

#if UNITY_ANDROID
        if (Permission.HasUserAuthorizedPermission(Permission.ExternalStorageWrite))
        {
            SaveAndSetWallpaper(texture);
        }
        else
        {
            Permission.RequestUserPermission(Permission.ExternalStorageWrite);
        }
#else
        Debug.Log("Wallpaper setting is only supported on Android for now.");
#endif
    }

    // Saves the texture to the device and sets it as the wallpaper
    void SaveAndSetWallpaper(Texture2D texture)
    {
        string path = Path.Combine(Application.persistentDataPath, "wallpaper.jpg");
        byte[] imageBytes = texture.EncodeToJPG();
        File.WriteAllBytes(path, imageBytes);

        AndroidJavaClass wallpaperManager = new AndroidJavaClass("android.app.WallpaperManager");
        AndroidJavaObject context = new AndroidJavaClass("com.unity3d.player.UnityPlayer")
            .GetStatic<AndroidJavaObject>("currentActivity");

        try
        {
            AndroidJavaObject wallpaperService = wallpaperManager.CallStatic<AndroidJavaObject>("getInstance", context);
            wallpaperService.Call("setStream", path);
            Debug.Log("Wallpaper set successfully!");
        }
        catch (System.Exception e)
        {
            Debug.LogError("Failed to set wallpaper: " + e.Message);
        }
    }
}
