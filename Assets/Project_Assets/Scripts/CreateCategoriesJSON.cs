using System.IO;
using UnityEngine;

public class CreateCategoriesJSON : MonoBehaviour
{
    private void Start()
    {
        // Define the path to the StreamingAssets folder
        string path = Path.Combine(Application.streamingAssetsPath, "categories.json");

        // Check if the file already exists to avoid overwriting
        if (!File.Exists(path))
        {
            // Define the JSON content
            string jsonContent = @"
            {
                ""categories"": [
                    ""Most Used"",
                    ""Newly Added"",
                    ""Weather"",
                    ""Nature"",
                    ""Abstract"",
                    ""Minimalist""
                ]
            }";

            // Ensure the StreamingAssets folder exists
            Directory.CreateDirectory(Application.streamingAssetsPath);

            // Create the file and write the content to it
            File.WriteAllText(path, jsonContent);

            Debug.Log($"categories.json created at: {path}");
        }
        else
        {
            Debug.Log("categories.json already exists.");
        }
    }
}
