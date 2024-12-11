using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class ImageDownloader : MonoBehaviour
{
    public string imageUrl = "https://drive.google.com/file/d/1eclhk6317aqdwwgkD9tW-QCEctjyrlDi/view?usp=sharing"; // Replace with your image URL
    public Image displayImage; // UI Image component to display the downloaded image
    public Button downloadButton; // Button to trigger the download

    private string filePath;

    private void Start()
    {

        // Attach the button click listener
        if (downloadButton != null)
        {
            downloadButton.onClick.AddListener(OnDownloadButtonClick);
        }
    }

    private void OnDownloadButtonClick()
    {
            StartCoroutine(DownloadAndSaveImage());
    }

    // Coroutine to download and save the image
    private IEnumerator DownloadAndSaveImage()
    {
        using (UnityWebRequest request = UnityWebRequestTexture.GetTexture(imageUrl))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("Image downloaded successfully!");

                // Get the downloaded texture
                Texture2D texture = DownloadHandlerTexture.GetContent(request);

                SaveImageToFile(texture);

                ApplyTextureToImage(texture);
            }
            else
            {
                Debug.LogError("Failed to download image: " + request.error);
            }
        }
    }

    // Save the texture as a file
    private void SaveImageToFile(Texture2D texture)
    {
        byte[] imageBytes = texture.EncodeToJPG(); // Convert texture to JPG format
        File.WriteAllBytes(filePath, imageBytes);
        Debug.Log("Image saved to: " + filePath);
    }

/*    // Load the image from the file and display it
    private void LoadImageFromFile()
    {
        byte[] imageBytes = File.ReadAllBytes(filePath);
        Texture2D texture = new Texture2D(2, 2);
        texture.LoadImage(imageBytes); // Load image data into the texture

        ApplyTextureToImage(texture);
    }*/

    private void ApplyTextureToImage(Texture2D texture)
    {
        Sprite sprite = Sprite.Create(
            texture,
            new Rect(0, 0, texture.width, texture.height),
            new Vector2(0.5f, 0.5f)
        );

        displayImage.sprite = sprite;
    }
}
