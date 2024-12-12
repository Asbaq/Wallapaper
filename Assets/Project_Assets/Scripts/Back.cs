using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class Back : MonoBehaviour
{
    public GameObject Panel; // Reference to the category selection panel
    public WallpaperDownload wallpaperDownload; // Reference to the category selection panel
    public string Paths; // Reference to the category selection panel

    private void Awake()
    {
        if (Panel == null)
        {
            Panel = GetComponentInParent<Image>().gameObject; // Find the WallpaperDownload instance
        }
            wallpaperDownload = FindObjectOfType<WallpaperDownload>();
    }

    private void BackButton()
    {
        Destroy(Panel);
    }

    public void Set()
    {
        Paths = Path.Combine(Application.persistentDataPath, "wallpaper.jpg");
        Debug.Log("wallpaperDownload.path" + Paths);
        wallpaperDownload.SetWallpaperAndroid(Paths);
    }

}
