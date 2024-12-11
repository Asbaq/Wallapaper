using System;
using UnityEngine;
using UnityEngine.UI;

public class WallpaperDownload : MonoBehaviour
{
    public GameObject Panel; // Prefab for the wallpaper detail panel
    public Canvas Canvas; // Reference to the main canvas
    private GameObject currentPanel; // Tracks the instantiated panel

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
            }
        }
    }
}
