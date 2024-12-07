using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.IO;

public class CategoryManager : MonoBehaviour
{
    [SerializeField] private Transform categoryContainer; // Parent object with Grid Layout Group
    [SerializeField] private GameObject categoryButtonPrefab; // Prefab for category buttons
    [SerializeField] private string categoriesJsonFileName = "categories.json"; // JSON file name

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

            // Populate the categories dynamically
            foreach (string category in categories)
            {
                CreateCategoryButton(category);
            }
        }
        else
        {
            Debug.LogError($"Categories file not found at {filePath}");
        }
    }

    private void CreateCategoryButton(string categoryName)
    {
        // Instantiate the category button prefab
        GameObject categoryButton = Instantiate(categoryButtonPrefab, categoryContainer);

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
            button.onClick.AddListener(() => OnCategorySelected(categoryName));
        }
    }

    private void OnCategorySelected(string categoryName)
    {
        Debug.Log($"Selected category: {categoryName}");
        // Navigate to the corresponding wallpaper list screen or perform an action
    }

    [System.Serializable]
    private class CategoryList
    {
        public List<string> categories;
    }
}
