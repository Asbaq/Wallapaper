using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SplashScreenManager : MonoBehaviour
{
    // Duration of the splash screen (in seconds)
    [SerializeField] private float splashDuration = 3f;

    // Name of the next scene (Home Page)
    [SerializeField] private string homeSceneName = "HomePage";

    private void Start()
    {
        // Start the coroutine to load the next scene after a delay
        StartCoroutine(LoadHomePageAfterDelay());
    }

    private IEnumerator LoadHomePageAfterDelay()
    {
        // Wait for the specified duration
        yield return new WaitForSeconds(splashDuration);

        // Load the Home Page scene
        SceneManager.LoadScene(homeSceneName);
    }
}
