using System;
using UnityEngine;
using UnityEngine.UI;

public class WallpaperDownload : MonoBehaviour
{
    public GameObject Panel;
    public GameObject Canvas;
  //  public GameObject Panel_1;

    private void Awake()
    {
        Canvas = FindObjectOfType<Canvas>().gameObject;
       // Panel_1 = GetComponentInChildren<Image>().gameObject;
    }

    public void Open()
    {
        GameObject wallpaperItem = Instantiate(Panel, Canvas.transform);
        wallpaperItem.GetComponent<Image>().sprite = gameObject.GetComponent<Image>().sprite;
    }
}
