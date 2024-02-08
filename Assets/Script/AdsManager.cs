using System;
using UnityEngine;
using GoogleMobileAds.Api;

public class AdsManager : MonoBehaviour
{
    static bool isAdsBannerLoaded = false;

    void Start()
    {
        if (!isAdsBannerLoaded)
            RequestBanner();
    }

    //광고 요청 Method
    private void RequestBanner()
    {
        //Test Banner ID : ca-app-pub-3940256099942544/6300978111
        //여러분들의 광고 ID가 들어갈 곳입니다.
        string BannerID = "ca-app-pub-3940256099942544/6300978111";
        BannerView bannerview = new BannerView(BannerID, AdSize.SmartBanner, AdPosition.Bottom);

        AdRequest request = new AdRequest.Builder().Build();
        bannerview.LoadAd(request);
        isAdsBannerLoaded = true;
    }


    /*
    private BannerView banner;

    public void Start()
    {
#if UNITY_ANDROID
        string appId = "ca-app-pub-3940256099942544~3347511713";
#elif UNITY_IPHONE
            string appId = "ca-app-pub-3940256099942544~1458002511";
#else
            string appId = "unexpected_platform";
#endif
        // Initialize the Google Mobile Ads SDK.
        MobileAds.Initialize(appId);

        this.RequestBanner();
    }

    private void RequestBanner()
    {
#if UNITY_ANDROID
        string AdUnitID = "ca-app-pub-3940256099942544/6300978111"; //테스트 아이디
#else
        string AdUnitID = "unDefind";
#endif

        banner = new BannerView(AdUnitID, AdSize.SmartBanner, AdPosition.Bottom);

        // Called when an ad request has successfully loaded.
        banner.OnAdLoaded += HandleOnAdLoaded_banner;
        // Called when an ad request failed to load.
        banner.OnAdFailedToLoad += HandleOnAdFailedToLoad_banner;
        // Called when an ad is clicked.
        banner.OnAdOpening += HandleOnAdOpened_banner;
        // Called when the user returned from the app after an ad click.
        banner.OnAdClosed += HandleOnAdClosed_banner;
        // Called when the ad click caused the user to leave the application.
        banner.OnAdLeavingApplication += HandleOnAdLeavingApplication_banner;

        AdRequest request = new AdRequest.Builder().Build();

        banner.LoadAd(request);
    }

    public void ShowBanner()
    {
        banner.Show();
    }

    public void HideBanner()
    {
        banner.Hide();
    }

    public void HandleOnAdLoaded_banner(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleAdLoaded event received_banner");
    }

    public void HandleOnAdFailedToLoad_banner(object sender, AdFailedToLoadEventArgs args)
    {
        MonoBehaviour.print("HandleFailedToReceiveAd_banner event received with message: "
                            + args.Message);
    }

    public void HandleOnAdOpened_banner(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleAdOpened event received_banner");
    }

    public void HandleOnAdClosed_banner(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleAdClosed event received_banner");
    }

    public void HandleOnAdLeavingApplication_banner(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleAdLeavingApplication event received_banner");
    }
    */
}