using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    [SerializeField] private Toggle fixedToggle;
    [SerializeField] private Toggle rotationalToggle;

    private void Start()
    {
        LoadPreferences();
        fixedToggle.onValueChanged.AddListener((isOn) => SetWallpaperType(isOn, "Fixed"));
        rotationalToggle.onValueChanged.AddListener((isOn) => SetWallpaperType(isOn, "Rotational"));
    }

    private void LoadPreferences()
    {
        string wallpaperType = PlayerPrefs.GetString("WallpaperType", "Fixed");
        fixedToggle.isOn = wallpaperType == "Fixed";
        rotationalToggle.isOn = wallpaperType == "Rotational";
    }

    private void SetWallpaperType(bool isOn, string type)
    {
        if (isOn)
        {
            PlayerPrefs.SetString("WallpaperType", type);
            PlayerPrefs.Save();
            Debug.Log($"Wallpaper Type Set to: {type}");
        }
    }
}
