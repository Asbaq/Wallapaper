using UnityEngine;

public class OfflineModeManager : MonoBehaviour
{
    public bool IsOffline => Application.internetReachability == NetworkReachability.NotReachable;

    void Update()
    {
        if (IsOffline)
        {
            Debug.Log("Offline Mode Enabled");
            // Restrict actions requiring internet
        }
    }
}
