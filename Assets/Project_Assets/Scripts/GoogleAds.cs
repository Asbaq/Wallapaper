using UnityEngine;
using GoogleMobileAds.Api;
using System.Collections.Generic;

public class GoogleAds : MonoBehaviour
{
    private BannerView bannerView;

    void Start()
    {
        // Initialize the Google Mobile Ads SDK
        MobileAds.Initialize(initStatus => { });
        RequestBanner();
    }

    private void RequestBanner()
    {
        string adUnitId = "YOUR_BANNER_AD_UNIT_ID"; // Replace with your actual ad unit ID

        // Create a BannerView and load the banner ad
        bannerView = new BannerView(adUnitId, AdSize.Banner, AdPosition.Bottom);

        // Create an ad request
        AdRequest request = new AdRequest();
        request.Keywords = new HashSet<string> { "gaming", "unity" }; // Optional: Add custom keywords
        bannerView.LoadAd(request);
    }
}
