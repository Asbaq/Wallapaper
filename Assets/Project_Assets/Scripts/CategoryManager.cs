using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine.Networking;
using System.Collections;

public class CategoryManager : MonoBehaviour
{
    [SerializeField] private Transform categoryContainer; // Parent object with Grid Layout Group
    [SerializeField] private GameObject categoryButtonPrefab; // Prefab for category buttons
    [SerializeField] private string categoriesJsonFileName = "categories.json"; // JSON file name
    [SerializeField] private WallpaperSelectionManager wallpaperSelectionManager;


    private void Awake()
    {
        wallpaperSelectionManager = FindObjectOfType<WallpaperSelectionManager>();
    }

    private void Start()
    {
        LoadCategories();
    }

    private void LoadCategories()
    {
        string filePath = Path.Combine(Application.streamingAssetsPath, categoriesJsonFileName);

        // For Android, use UnityWebRequest or WWW to load the file
        if (filePath.StartsWith("jar:file://"))
        {
            // Android uses this path scheme when accessing the assets inside the APK
            StartCoroutine(LoadCategoriesFromStreamingAssets(filePath));
        }
        else
        {
            // Non-Android platforms can directly use File.ReadAllText
            if (File.Exists(filePath))
            {
                string json = File.ReadAllText(filePath);
                ProcessCategoriesJson(json);
            }
            else
            {
                Debug.LogError($"Categories file not found at {filePath}");
            }
        }
    }

    private IEnumerator LoadCategoriesFromStreamingAssets(string filePath)
    {
        using (UnityWebRequest www = UnityWebRequest.Get(filePath))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"Failed to load categories file: {www.error}");
            }
            else
            {
                string json = www.downloadHandler.text;
                ProcessCategoriesJson(json);
            }
        }
    }

    private void ProcessCategoriesJson(string json)
    {
        List<string> categories = JsonUtility.FromJson<CategoryList>(json).categories;

        for (int i = 0; i < categories.Count; i++)
        {
            Debug.Log("Category: " + categories[i]);
            CreateCategoryButton(categories[i], i);
        }
    }

    private void CreateCategoryButton(string categoryName, int id)
    {
        // Instantiate the category button prefab
        GameObject categoryButton = Instantiate(categoryButtonPrefab, categoryContainer);
        categoryButton.GetComponent<ID>().id = id;
        categoryButton.GetComponentInChildren<TextMeshProUGUI>().text = categoryName;

        // Add a click listener to handle category selection
        Button button = categoryButton.GetComponent<Button>();
        if (button != null)
        {
            button.onClick.AddListener(() => OnCategorySelected(categoryName,id));
        }
    }

    private void OnCategorySelected(string categoryName,int ID)
    {
        wallpaperSelectionManager.LoadWallpapers(ID);
    }

    [System.Serializable]
    private class CategoryList
    {
        public List<string> categories;
    }

}
