using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;

public class WallpaperSelectionManager : MonoBehaviour
{
    [SerializeField] private Transform wallpaperContainer;
    [SerializeField] private GameObject wallpaperPrefab;
    [SerializeField] private Button setFixedButton;
    [SerializeField] private Button setRotationalButton;

    private string selectedWallpaperUrl;

    private void Start()
    {
        LoadWallpapers();
        ConfigureButtons();
    }

    private void LoadWallpapers()
    {
        string[] wallpaperUrls = { "https://example.com/wallpaper1.jpg", "https://example.com/wallpaper2.jpg" };

        foreach (string url in wallpaperUrls)
        {
            StartCoroutine(LoadWallpaper(url));
        }
    }

    private IEnumerator LoadWallpaper(string url)
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
    }

    private void ConfigureButtons()
    {
        setFixedButton.onClick.AddListener(() => SetWallpaper(false));
        setRotationalButton.onClick.AddListener(() => SetWallpaper(true));
    }

    private void SetWallpaper(bool isRotational)
    {
        Debug.Log($"Setting wallpaper as {(isRotational ? "Rotational" : "Fixed")}: {selectedWallpaperUrl}");
        PlayerPrefs.SetString("WallpaperType", isRotational ? "Rotational" : "Fixed");
        PlayerPrefs.SetString("SelectedWallpaper", selectedWallpaperUrl);
        PlayerPrefs.Save();
    }
}
