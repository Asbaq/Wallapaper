using UnityEngine;
using UnityEngine.UI;

public class Back : MonoBehaviour
{
    public GameObject Panel; // Reference to the category selection panel

    private void Awake()
    {
        if (Panel == null)
        {
            Panel = GetComponentInParent<Image>().gameObject; // Find the WallpaperDownload instance
        }
    }

    private void BackButton()
    {
        Destroy(Panel);
    }

}
