using System;
using System.Collections;
using GoogleMobileAds.Api;
using UnityEngine;

public class AdController : MonoBehaviour
{
    [Header("App Identities")]
    public string ANDROID_APP_ID;
    public string IOS_APP_ID;

    [Header("Interstitial")]
    public bool InterstitialActive;
    public string Interstitial_ANDROID;
    public string Interstitial_IOS;

    [Header("Banner")]
    public bool BannerActive;
    public AdPosition BannerPosition = AdPosition.Top;
    public string Banner_ANDROID;
    public string Banner_IOS;

    [Header("Other Settings")]
    public bool ForChildren = false;
    public string[] TestDeviceIds;

    private InterstitialAd _interstitial;
    private BannerView _banner;
    private IEnumerator _showInterstitialCoroutine;
    private float _timeoutForinterstitialRequest;

    private void Start()
    {
        DontDestroyOnLoad(this);

#if UNITY_ANDROID
        MobileAds.Initialize(ANDROID_APP_ID);
#elif UNITY_IOS
        MobileAds.SetiOSAppPauseOnBackground(true);
        MobileAds.Initialize(IOS_APP_ID);
#endif
        MobileAds.SetApplicationMuted(true);

        if (InterstitialActive)
            LoadInterstitial();

        if (BannerActive)
            LoadBanner();

    }

    private AdRequest BuildRequest()
    {
        AdRequest.Builder builder = new AdRequest.Builder();

        if (ForChildren)
            builder.TagForChildDirectedTreatment(true);

        if (TestDeviceIds != null)
        {
            foreach (var id in TestDeviceIds)
            {
                builder.AddTestDevice(id);
            }
        }

        return builder.Build();
    }

    #region "Banner"

    public void LoadBanner()
    {
        if (_banner != null)
            _banner.Destroy();

        if (Debug.isDebugBuild)
        {
#if UNITY_ANDROID
            _banner = new BannerView( "ca-app-pub-3940256099942544/6300978111", AdSize.SmartBanner, BannerPosition);
#elif UNITY_IOS
            _banner = new BannerView("ca-app-pub-3940256099942544/2934735716", AdSize.SmartBanner, BannerPosition);
#endif
        }
        else
        {
#if UNITY_ANDROID
            _banner = new BannerView(Banner_ANDROID, AdSize.SmartBanner, BannerPosition);
#elif UNITY_IOS
            _banner = new BannerView(Banner_IOS, AdSize.SmartBanner, BannerPosition);
#else
            _banner = new BannerView("ca-app-pub-3940256099942544/6300978111", AdSize.SmartBanner, BannerPosition);
#endif
        }
        _banner.OnAdFailedToLoad += BannerFailedCallback;
        _banner.LoadAd(BuildRequest());
        _banner.Hide();
    }


    public void BannerFailedCallback(object sender, AdFailedToLoadEventArgs args)
    {
        if (_banner != null)
        {
            _banner.Destroy();
            _banner = null;
        }
    }

    public void ShowBanner()
    {
        if (_banner == null)
        {
            LoadBanner();
            if (_banner == null)
                return;
        }
        _banner.Show();
    }

    public void SetBannerPosition(int position)
    {
        if ( _banner != null)
            _banner.SetPosition((AdPosition)position);
    }

    public void DestroyBanner()
    {
        HideBanner();
        _banner = null;
    }

    public void HideBanner()
    {
        if (_banner == null)
            return;

        _banner.Hide();
    }

    #endregion

    #region "Interstitial"

    private void LoadInterstitial()
    {
        if (_interstitial != null)
            _interstitial.Destroy();

        if (Debug.isDebugBuild)
        {
#if UNITY_ANDROID
            _interstitial = new InterstitialAd("ca-app-pub-3940256099942544/1033173712");
#elif UNITY_IOS
            _interstitial = new InterstitialAd("ca-app-pub-3940256099942544/4411468910");
#else
            _interstitial = new InterstitialAd("\"ca-app-pub-3940256099942544/1033173712");
#endif
        }
        else
        {
#if UNITY_ANDROID
            _interstitial = new InterstitialAd(Interstitial_ANDROID);
#elif UNITY_IOS
            _interstitial = new InterstitialAd(Interstitial_IOS);
#endif
        }

        _interstitial.OnAdClosed += InterstitialClosedCallback;
        _interstitial.OnAdFailedToLoad += InterstitialFailedCallback;
        _interstitial.LoadAd(BuildRequest());
        _timeoutForinterstitialRequest = Time.realtimeSinceStartup + 10f;
    }

    public void ShowInsterstitial()
    {
        if (_interstitial == null)
        {
            LoadInterstitial();
            if (_interstitial == null)
                return;
        }

        if (_showInterstitialCoroutine != null)
        {
            StopCoroutine(_showInterstitialCoroutine);
            _showInterstitialCoroutine = null;
        }

        if (_interstitial.IsLoaded())
            _interstitial.Show();
        else
        {
            if (Time.realtimeSinceStartup >= _timeoutForinterstitialRequest)
                LoadInterstitial();

            _showInterstitialCoroutine = ShowInsterstitialCoroutine();
            StartCoroutine(_showInterstitialCoroutine);
        }
    }

    private IEnumerator ShowInsterstitialCoroutine()
    {
        float timeoutWhen = Time.realtimeSinceStartup + 2.5f;
        while (!_interstitial.IsLoaded())
        {
            if (Time.realtimeSinceStartup > timeoutWhen)
                yield break;

            yield return null;

            if (_interstitial == null)
                yield break;
        }

        _interstitial.Show();
    }

    private void InterstitialClosedCallback(object sender, EventArgs args)
    {
        LoadInterstitial();
    }

    private void InterstitialFailedCallback(object sender, AdFailedToLoadEventArgs args)
    {
        if (_interstitial != null)
        {
            _interstitial.Destroy();
            _interstitial = null;
        }
    }

    #endregion

}
