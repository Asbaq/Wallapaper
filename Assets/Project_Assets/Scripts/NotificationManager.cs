using Firebase.Messaging;
using UnityEngine;

public class NotificationManager : MonoBehaviour
{
    void Start()
    {
        FirebaseMessaging.MessageReceived += OnMessageReceived;
        FirebaseMessaging.TokenReceived += OnTokenReceived;
    }

    private void OnMessageReceived(object sender, Firebase.Messaging.MessageReceivedEventArgs e)
    {
        Debug.Log($"Notification received: {e.Message.Notification.Title} - {e.Message.Notification.Body}");
    }

    private void OnTokenReceived(object sender, Firebase.Messaging.TokenReceivedEventArgs e)
    {
        Debug.Log($"Firebase Token: {e.Token}");
    }

    private void OnDestroy()
    {
        FirebaseMessaging.MessageReceived -= OnMessageReceived;
        FirebaseMessaging.TokenReceived -= OnTokenReceived;
    }
}
