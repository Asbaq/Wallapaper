using UnityEngine;
using GoogleMobileAds.Api;

public class InterstitialAdManager : MonoBehaviour
{
    private InterstitialAd interstitialAd;

    void Start()
    {
        RequestInterstitial();
    }

    private void RequestInterstitial()
    {
        string adUnitId = "YOUR_INTERSTITIAL_AD_UNIT_ID";

        // Load the interstitial ad
        InterstitialAd.Load(adUnitId, new AdRequest(), HandleAdLoaded);
    }

    private void HandleAdLoaded(InterstitialAd ad, LoadAdError error)
    {
        if (error != null)
        {
            Debug.LogError("Failed to load interstitial ad: " + error);
            return;
        }

        interstitialAd = ad;

        // Register events
        interstitialAd.OnAdFullScreenContentOpened += HandleAdOpened;
        interstitialAd.OnAdFullScreenContentClosed += HandleAdClosed;
        interstitialAd.OnAdFullScreenContentFailed += HandleAdFailed;
    }

    public void ShowInterstitialAd()
    {
        if (interstitialAd != null && interstitialAd.CanShowAd())
        {
            interstitialAd.Show();
        }
        else
        {
            Debug.Log("Interstitial ad is not ready yet.");
        }
    }

    private void HandleAdOpened()
    {
        Debug.Log("Interstitial ad opened.");
    }

    private void HandleAdClosed()
    {
        Debug.Log("Interstitial ad closed.");
        RequestInterstitial();  // Pre-load the next ad after one is closed
    }

    private void HandleAdFailed(AdError adError)
    {
        Debug.LogError("Interstitial ad failed to show: " + adError);
    }

    private void OnDestroy()
    {
        if (interstitialAd != null)
        {
            interstitialAd.Destroy();
        }
    }
}
