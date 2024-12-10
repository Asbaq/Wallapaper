using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;

public class WallpaperSelectionManager : MonoBehaviour
{
    [SerializeField] private Transform wallpaperContainer;
    [SerializeField] private GameObject wallpaperPrefab;
    [SerializeField] private Button setFixedButton;
    [SerializeField] private Button setRotationalButton;

    private string selectedWallpaperName;
    [SerializeField] private List<Sprite> offlineWallpapers; // List of offline images

    private void Start()
    {
        LoadWallpapers();
        ConfigureButtons();
    }

    private void LoadWallpapers()
    {
        foreach (Sprite wallpaper in offlineWallpapers)
        {
            CreateWallpaperItem(wallpaper);
        }
    }

    /*    private void LoadWallpapers()
    {

        string[] wallpaperUrls = { "https://example.com/wallpaper1.jpg", "https://example.com/wallpaper2.jpg" };

        string[] wallpaper = { };

        foreach (string url in wallpaper)
        {
            StartCoroutine(LoadWallpaper(url));
        }
    }*/

    /*    private IEnumerator LoadWallpaper(string url)
        {
            UnityWebRequest request = UnityWebRequestTexture.GetTexture(url);
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                Texture2D texture = DownloadHandlerTexture.GetContent(request);
                GameObject wallpaperItem = Instantiate(wallpaperPrefab, wallpaperContainer);
                wallpaperItem.GetComponentInChildren<Image>().sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
                wallpaperItem.GetComponent<Button>().onClick.AddListener(() => OnWallpaperSelected(url));
            }
            else
            {
                Debug.LogError($"Failed to load wallpaper: {url}");
            }
        }

        private void OnWallpaperSelected(string url)
        {
            selectedWallpaperUrl = url;
            Debug.Log($"Selected Wallpaper: {url}");
        }*/

    private void CreateWallpaperItem(Sprite wallpaperSprite)
    {
        GameObject wallpaperItem = Instantiate(wallpaperPrefab, wallpaperContainer);
        wallpaperItem.GetComponentInChildren<Image>().sprite = wallpaperSprite;
        wallpaperItem.GetComponent<Button>().onClick.AddListener(() => OnWallpaperSelected(wallpaperSprite.name));
    }

    private void OnWallpaperSelected(string wallpaperName)
    {
        selectedWallpaperName = wallpaperName;
        Debug.Log($"Selected Wallpaper: {wallpaperName}");
    }

    private void ConfigureButtons()
    {
        setFixedButton.onClick.AddListener(() => SetWallpaper(false));
        setRotationalButton.onClick.AddListener(() => SetWallpaper(true));
    }

    private void SetWallpaper(bool isRotational)
    {
        Debug.Log($"Setting wallpaper as {(isRotational ? "Rotational" : "Fixed")}: {selectedWallpaperName}");
        PlayerPrefs.SetString("WallpaperType", isRotational ? "Rotational" : "Fixed");
        PlayerPrefs.SetString("SelectedWallpaper", selectedWallpaperName);
        PlayerPrefs.Save();
    }
}
