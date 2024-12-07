using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;

public class NewsletterManager : MonoBehaviour
{
    [SerializeField] private InputField nameInputField;
    [SerializeField] private InputField emailInputField;
    [SerializeField] private Button submitButton;

    private void Start()
    {
        submitButton.onClick.AddListener(SubmitSubscription);
    }

    private void SubmitSubscription()
    {
        string name = nameInputField.text;
        string email = emailInputField.text;

        if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(email))
        {
            Debug.LogWarning("Name and Email are required.");
            return;
        }

        StartCoroutine(SendSubscriptionData(name, email));
    }

    private IEnumerator SendSubscriptionData(string name, string email)
    {
        WWWForm form = new WWWForm();
        form.AddField("name", name);
        form.AddField("email", email);

        UnityWebRequest request = UnityWebRequest.Post("https://your-backend-url.com/subscribe", form);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("Subscription successful!");
        }
        else
        {
            Debug.LogError($"Subscription failed: {request.error}");
        }
    }
}
