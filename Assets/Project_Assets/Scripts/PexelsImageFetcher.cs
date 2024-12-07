using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class PexelsImageFetcher : MonoBehaviour
{
    private string apiUrl = "https://api.pexels.com/v1/curated?page=1&per_page=1";
    private string apiKey = "iu37O7LyVgIeCNzLoSQwnJXOgFl7MQG8GzbVo4aea95EbrSjk5YkXFZn";

    void Start()
    {
        StartCoroutine(FetchImage());
    }

    IEnumerator FetchImage()
    {
        UnityWebRequest webRequest = UnityWebRequest.Get(apiUrl);
        webRequest.SetRequestHeader("Authorization", apiKey); // Add API Key

        yield return webRequest.SendWebRequest();

        if (webRequest.result == UnityWebRequest.Result.Success)
        {
            string jsonResponse = webRequest.downloadHandler.text;
            string imageUrl = ExtractImageUrl(jsonResponse);
            StartCoroutine(DownloadImage(imageUrl));
        }
        else
        {
            Debug.LogError("Error fetching image: " + webRequest.error);
        }
    }

    string ExtractImageUrl(string jsonResponse)
    {
        int startIndex = jsonResponse.IndexOf("\"url\":\"") + 7;
        int endIndex = jsonResponse.IndexOf("\"", startIndex);
        return jsonResponse.Substring(startIndex, endIndex - startIndex);
    }

    IEnumerator DownloadImage(string imageUrl)
    {
        UnityWebRequest webRequest = UnityWebRequestTexture.GetTexture(imageUrl);
        yield return webRequest.SendWebRequest();

        if (webRequest.result == UnityWebRequest.Result.Success)
        {
            Texture2D texture = ((DownloadHandlerTexture)webRequest.downloadHandler).texture;
            GetComponent<Renderer>().material.mainTexture = texture; // Apply to object
        }
        else
        {
            Debug.LogError("Error downloading image: " + webRequest.error);
        }
    }
}
