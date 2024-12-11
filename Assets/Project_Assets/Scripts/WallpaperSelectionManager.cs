using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;

public class WallpaperSelectionManager : MonoBehaviour
{
    [SerializeField] private Transform wallpaperContainer;
    [SerializeField] private GameObject wallpaperPrefab;
/*    [SerializeField] private Button setFixedButton;
    [SerializeField] private Button setRotationalButton;*/

    private string selectedWallpaperName;
    [SerializeField] private List<Sprite> offlineWallpapers; // List of offline images
    [SerializeField] private List<GameObject> Wallpapers; // List of offline images
    private readonly List<GameObject> wallpaperItems = new List<GameObject>(); // Dynamic list of instantiated wallpaper items

    private void Start()
    {
        LoadWallpapers(0);
    }

    /// <summary>
    /// Loads wallpapers into the UI container dynamically.
    /// </summary>
    public void LoadWallpapers(int ButtonID)
    {
        ClearExistingWallpapers();

        for(int i = 0; i<offlineWallpapers.Count; i++)
        { 
            CreateWallpaperItem(offlineWallpapers[ButtonID]);
        }
    }

    /// <summary>
    /// Clears existing wallpaper items to prevent duplicates.
    /// </summary>
    private void ClearExistingWallpapers()
    {
        foreach (var item in wallpaperItems)
        {
            Destroy(item);
        }

        wallpaperItems.Clear();
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

    /// <summary>
    /// Creates a UI item for the given wallpaper.
    /// </summary>
    /// <param name="wallpaperSprite">The sprite to display in the wallpaper item.</param>
    private void CreateWallpaperItem(Sprite wallpaperSprite)
    {
        var wallpaperItem = Instantiate(wallpaperPrefab, wallpaperContainer);
        wallpaperItems.Add(wallpaperItem);

        // Set wallpaper sprite
        var wallpaperImage = wallpaperItem.GetComponentInChildren<Image>();
        wallpaperImage.sprite = wallpaperSprite;

        // Add click functionality
        var button = wallpaperItem.GetComponent<Button>();
        button.onClick.AddListener(() => OnWallpaperSelected(wallpaperSprite.name));
    }

    /// <summary>
    /// Called when a wallpaper is selected.
    /// </summary>
    /// <param name="wallpaperName">The name of the selected wallpaper.</param>
    private void OnWallpaperSelected(string wallpaperName)
    {
        selectedWallpaperName = wallpaperName;
        Debug.Log($"Selected Wallpaper: {wallpaperName}");

        // Optional: Transition to full-screen wallpaper view or open a detailed options menu
        OpenFullImageView(wallpaperName);
    }

    /// <summary>
    /// Opens a full image view for the selected wallpaper.
    /// </summary>
    /// <param name="wallpaperName">The name of the selected wallpaper.</param>
    private void OpenFullImageView(string wallpaperName)
    {
        // Placeholder: Replace with your full-screen image logic
        Debug.Log($"Opening full image view for: {wallpaperName}");
    }

    /*   private void ConfigureButtons()
       {
           setFixedButton.onClick.AddListener(() => SetWallpaper(false));
           setRotationalButton.onClick.AddListener(() => SetWallpaper(true));
       }*/

    /*   private void SetWallpaper(bool isRotational)
       {
           Debug.Log($"Setting wallpaper as {(isRotational ? "Rotational" : "Fixed")}: {selectedWallpaperName}");
           PlayerPrefs.SetString("WallpaperType", isRotational ? "Rotational" : "Fixed");
           PlayerPrefs.SetString("SelectedWallpaper", selectedWallpaperName);
           PlayerPrefs.Save();
       }*/
}
