using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;
using System;
using Unity.Profiling;

public class GoogleMobileVideoAdsScript : MonoBehaviour
{
    static GameObject go;

    public static GoogleMobileVideoAdsScript instance;

    //public static GoogleMobileVideoAdsScript Instance
    //{
    //    get
    //    {
    //        if (instance == null)
    //        {
    //            go = new GameObject();
    //            go.name = "GoogleMobileVideoAdsScript";
    //            instance = go.AddComponent<GoogleMobileVideoAdsScript>() as GoogleMobileVideoAdsScript;   //스크립트 첨부하면서 변수로 활용

    //            DontDestroyOnLoad(go);  //scene 로드시, 파괴하지 않을 오브젝트 설정 (ex> scene 전환)
    //        }
    //        return instance;
    //    }
    //}

    string adUnitId;
    public InterstitialAd interstitialAd;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        // Initialize the Google Mobile Ads SDK.
        MobileAds.Initialize((InitializationStatus initStatus) =>
        {
            // This callback is called once the MobileAds SDK is initialized.
        });

        //DontDestroyOnLoad(gameObject);

#if UNITY_ANDROID
         adUnitId = "ca-app-pub-3940256099942544/1033173712";    //android 전면 광고 테스트용
                                                                       //string _adUnitId = "ca-app-pub-4036723639800426/3835284751";    //실제용
#elif UNITY_IPHONE
      adUnitId = "ca-app-pub-3940256099942544/4411468910";
#else
      adUnitId = "unexpected_platform";
#endif
    }

    public void LoadInterstitialAd()
    {
        if (interstitialAd != null)
        {
            interstitialAd.Destroy();
            interstitialAd = null;
        }

        var adRequest = new AdRequest.Builder().AddKeyword("unity-admob-sample").Build();
        InterstitialAd.Load(adUnitId, adRequest, (InterstitialAd ad, LoadAdError error) =>
        {
            if (ad == null || error != null)
            {
                return;
            }

            interstitialAd = ad;
        });

        RegisterEventHandlers(interstitialAd);
    }

    public void ShowAd()
    {
        if (interstitialAd != null && interstitialAd.CanShowAd())
        {
            interstitialAd.Show();
        }
        else
        {
            LoadInterstitialAd();   //광고 리로드
        }
    }

    private void RegisterEventHandlers(InterstitialAd ad) //광고 이벤트
    {
        ad.OnAdPaid += (AdValue adValue) =>
        {

            //보상 주기

            Debug.Log(string.Format("Interstitial ad paid {0} {1}.",
                adValue.Value,
                adValue.CurrencyCode));
        };
        ad.OnAdImpressionRecorded += () =>
        {
            Debug.Log("Interstitial ad recorded an impression.");
        };
        ad.OnAdClicked += () =>
        {
            Debug.Log("Interstitial ad was clicked.");
        };
        ad.OnAdFullScreenContentOpened += () =>
        {
            Debug.Log("Interstitial ad full screen content opened.");
        };
        ad.OnAdFullScreenContentClosed += () =>
        {
            Debug.Log("Interstitial ad full screen content closed.");
        };
        ad.OnAdFullScreenContentFailed += (AdError error) =>
        {
            Debug.LogError("Interstitial ad failed to open full screen content " +
                           "with error : " + error);
        };
    }
}
