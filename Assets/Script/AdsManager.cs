using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;
using Unity.VisualScripting;
using System;

public class AdsManager : MonoBehaviour
{
    private BannerView bannerView;

    public void Start()
    {
        MobileAds.Initialize((InitializationStatus initStatus) => { });
    }
#if UNITY_ANDROID
    string adUnitId = "ca-app-pub-3940256099942544/6300978111";

#elif UNITY_IPHON
     string adUnitId = "ca-app-pub-3940256099942544/2934735716";
#else
       string adUnitId = "unexpected_platform";
    
#endif


    public void CreateBannerView()
    {
        Debug.Log("Creating banner view");

        if (bannerView != null)
        {
            DestroyBannerView();
        }

        bannerView = new BannerView(adUnitId, AdSize.Banner, AdPosition.BottomLeft);

        //AdSize adSize = new AdSize(0, 0);
        //bannerView = new BannerView(adUnitId, adSize, AdPosition.Bottom);
    }

    public void LoadAd()
    {
        if(bannerView ==null)
        {
            CreateBannerView();
        }

        var adRequest = new AdRequest();

        Debug.Log("Loading banner ad");
        bannerView.LoadAd(adRequest);
    }

    private void ListenToAdEvents()
    {
        bannerView.OnBannerAdLoaded += () =>
        {
            Debug.Log("Banner view loaded an ad with response : " + bannerView.GetResponseInfo());
        };

        bannerView.OnBannerAdLoadFailed += (LoadAdError error) =>
        {
            Debug.LogError("Banner view failed to load an ad with error: " + error);
        };

        bannerView.OnAdPaid += (AdValue adValue) =>
        {
            Debug.Log(String.Format("Banner view paid {0} {1}.", adValue.Value, adValue.CurrencyCode));
        };

        bannerView.OnAdImpressionRecorded += () =>
        {
            Debug.Log("Banner view recorded an impression.");
        };

        bannerView.OnAdClicked += () =>
        {
            Debug.Log("Banner view was clicked.");
        };

        bannerView.OnAdFullScreenContentOpened += () =>
        {
            Debug.Log("Banner view full screen content opened.");
        };

        bannerView.OnAdFullScreenContentClosed += () =>
        {
            Debug.Log("Banner view full screen content closed.");
        };
    }

    public void DestroyBannerView()
    {
        if(bannerView !=null)
        {
            Debug.Log("Destroying bannerview");
            bannerView.Destroy();
            bannerView = null;
        }
    }



}
