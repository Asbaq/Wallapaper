using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.IO;

public class CategoryManager : MonoBehaviour
{
    [SerializeField] private Transform categoryContainer; // Parent object with Grid Layout Group
    [SerializeField] private GameObject categoryButtonPrefab; // Prefab for category buttons
  //  [SerializeField] private List<GameObject> categoryButtonPrefabs; // Prefab for category buttons
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
        // Load the JSON file from the Resources folder or a persistent location
        string filePath = Path.Combine(Application.streamingAssetsPath, categoriesJsonFileName);
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            List<string> categories = JsonUtility.FromJson<CategoryList>(json).categories;

            for (int i = 0; i < categories.Count; i++)
            {
                Debug.Log("category " + categories[i]);
                CreateCategoryButton(categories[i],i);
                Debug.Log(i);
            }
        }
        else
        {
            Debug.LogError($"Categories file not found at {filePath}");
        }
    }

    private void CreateCategoryButton(string categoryName, int id)
    {
        // Instantiate the category button prefab
        GameObject categoryButton = Instantiate(categoryButtonPrefab, categoryContainer);
        categoryButton.GetComponent<ID>().id = id;
        Debug.Log(id);
        // Set the text to the category name
        Text categoryText = categoryButton.GetComponentInChildren<Text>();
        if (categoryText != null)
        {
            categoryText.text = categoryName;
        }

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
        Debug.Log($"Selected category: {categoryName}");
    }

    [System.Serializable]
    private class CategoryList
    {
        public List<string> categories;
    }
}
