using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds;
using GoogleMobileAds.Api;
using System;
using Unity.VisualScripting;
using UnityEditor.Advertisements;

public class GoogleMobileBannerAdsScript : MonoBehaviour
{
    void Start()
    {
        // Initialize the Google Mobile Ads SDK.
        MobileAds.Initialize((InitializationStatus initStatus) =>
        {
            // This callback is called once the MobileAds SDK is initialized.
        });
    }

    private void OnEnable()
    {
        LoadAd();
    }

    private void OnDisable()
    {
        _bannerView.Hide();
    }

    // These ad units are configured to always serve test ads.
#if UNITY_ANDROID
    private string _adUnitId = "ca-app-pub-3940256099942544/6300978111";    //android 배너 광고 테스트용
    //private string _adUnitId = "ca-app-pub-4036723639800426/8051542579";    //실제용
#elif UNITY_IPHONE
  private string _adUnitId = "ca-app-pub-3940256099942544/2934735716";
#else
  private string _adUnitId = "unexpected_platform";
#endif

    BannerView _bannerView;

    public void CreateBannerView()
    {
        Debug.Log("Creating banner view");

        // 기존 320x50 배너 생성
        _bannerView = new BannerView(_adUnitId, AdSize.Banner, AdPosition.Bottom);

        //기종 해상도에 맞는 사이즈 배너 생성
        AdSize adaptiveSize = AdSize.GetCurrentOrientationAnchoredAdaptiveBannerAdSizeWithWidth(AdSize.FullWidth);
        this._bannerView = new BannerView(_adUnitId, adaptiveSize, AdPosition.Bottom);
    }

    /// <summary>
    /// Creates the banner view and loads a banner ad.
    /// </summary>
    public void LoadAd()
    {
        // create an instance of a banner view first.
        if (_bannerView == null)
        {
            CreateBannerView();
        }

        // create our request used to load the ad.
        AdRequest adRequest = new AdRequest();

        // send the request to load the ad.
        Debug.Log("Loading banner ad.");
        _bannerView.LoadAd(adRequest);
    }

    /// <summary>
    /// listen to events the banner view may raise.
    /// </summary>
    //private void ListenToAdEvents()
    //{
    //    // Raised when an ad is loaded into the banner view.
    //    _bannerView.OnBannerAdLoaded += () =>
    //    {
    //        Debug.Log("Banner view loaded an ad with response : "
    //            + _bannerView.GetResponseInfo());
    //    };
    //    // Raised when an ad fails to load into the banner view.
    //    _bannerView.OnBannerAdLoadFailed += (LoadAdError error) =>
    //    {
    //        Debug.LogError("Banner view failed to load an ad with error : "
    //            + error);
    //    };
    //    // Raised when the ad is estimated to have earned money.
    //    _bannerView.OnAdPaid += (AdValue adValue) =>
    //    {
    //        Debug.Log(String.Format("Banner view paid {0} {1}.",
    //            adValue.Value,
    //            adValue.CurrencyCode));
    //    };
    //    // Raised when an impression is recorded for an ad.
    //    _bannerView.OnAdImpressionRecorded += () =>
    //    {
    //        Debug.Log("Banner view recorded an impression.");
    //    };
    //    // Raised when a click is recorded for an ad.
    //    _bannerView.OnAdClicked += () =>
    //    {
    //        Debug.Log("Banner view was clicked.");
    //    };
    //    // Raised when an ad opened full screen content.
    //    _bannerView.OnAdFullScreenContentOpened += () =>
    //    {
    //        Debug.Log("Banner view full screen content opened.");
    //    };
    //    // Raised when the ad closed full screen content.
    //    _bannerView.OnAdFullScreenContentClosed += () =>
    //    {
    //        Debug.Log("Banner view full screen content closed.");
    //    };
    //}

    /// <summary>
    /// Destroys the banner view.
    /// </summary>
    //public void DestroyBannerView()
    //{
    //    if (_bannerView != null)
    //    {
    //        Debug.Log("Destroying banner view.");
    //        _bannerView.Destroy();
    //        _bannerView = null;
    //    }
    //}
}
