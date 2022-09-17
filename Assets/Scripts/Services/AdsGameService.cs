using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Advertisements;

public class AdsGameService : IUnityAdsInitializationListener, IUnityAdsLoadListener, IUnityAdsShowListener,
    IService
{
    private string _adsGameId;
    private string _adUnitId;

    public bool IsAdReady => Advertisement.IsReady(_adUnitId);
    private Action<bool> _adWatched = null;

    public AdsGameService(string adsGameId, string adUnitId)
    {
        _adsGameId = adsGameId;
        _adUnitId = adUnitId;
    }

    public void Initialize(bool testMode = false)
    {
        Advertisement.Initialize(_adsGameId, testMode, true, this);
    }

    public void OnInitializationComplete()
    {
        Debug.Log("Unity Ads initialization complete.");
        LoadAd();
    }

    public void OnInitializationFailed(UnityAdsInitializationError error, string message)
    {
        Debug.Log($"Unity Ads Initialization Failed: {error.ToString()} - {message}");
    }

    public void LoadAd()
    {
        Debug.Log("Loading Ad: " + _adUnitId);
        Advertisement.Load(_adUnitId, this);
    }

    public void OnUnityAdsAdLoaded(string adUnitId)
    {
        Debug.Log("Ad Loaded: " + adUnitId);
    }

    public void ShowAd(Action<bool> onFinished)
    {
        _adWatched = onFinished;
        Advertisement.Show(_adUnitId, this);
#if UNITY_EDITOR
        DelayedDebugWatch();
#endif
    }

    private async void DelayedDebugWatch()
    {
        await Task.Delay(2000);
        OnUnityAdsShowComplete(_adUnitId, UnityAdsShowCompletionState.COMPLETED);
    }

    public void OnUnityAdsShowComplete(string adUnitId, UnityAdsShowCompletionState showCompletionState)
    {
        Debug.Log("Unity Ads Rewarded Ad:" + showCompletionState.ToString());
        Advertisement.Load(_adUnitId, this);
        _adWatched?.Invoke(showCompletionState == UnityAdsShowCompletionState.COMPLETED);
        _adWatched = null;
    }

    public void OnUnityAdsFailedToLoad(string adUnitId, UnityAdsLoadError error, string message)
    {
        Debug.Log($"Error loading Ad Unit {adUnitId}: {error.ToString()} - {message}");
        Advertisement.Load(_adUnitId, this);
    }

    public void OnUnityAdsShowFailure(string adUnitId, UnityAdsShowError error, string message)
    {
        Debug.Log($"Error showing Ad Unit {adUnitId}: {error.ToString()} - {message}");
        Advertisement.Load(_adUnitId, this);
    }

    public void OnUnityAdsShowStart(string adUnitId)
    {
        Debug.Log("Started watching an ad");
    }

    public void OnUnityAdsShowClick(string adUnitId)
    {
        Debug.Log("User clicked in the ad");
    }

    public void Clear()
    {
    }
}