using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;
using System;
using UnityEngine.UI;

public class AdsManager : MonoBehaviour, IUnityAdsListener
{
    #region Singleton
    public static AdsManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(this.gameObject);
        }
    }
    #endregion

    private static bool isAdFree = false;
    [SerializeField] private Text toggleAdText;

    public string gameID
    {
        get
        {
            #if UNITY_ANDROID
                return "3986337";
            #elif UNITY_IOS
                return "3986336";
            #endif
        }
    }

    public const string rewardVideoAdID = "rewardedVideo";
    public const string videoAdID = "video";
    public const string bannerAdID = "banner";

    public event EventHandler<AdFinishEventArgs> OnAdDone;

    private void Start()
    {
        Advertisement.Initialize(gameID, true);
        Advertisement.AddListener(this);
        ShowBannerAd();
    }

    public void OnUnityAdsReady(string placementId)
    {
        Debug.Log("Ads Ready");
    }

    public void OnUnityAdsDidError(string message)
    {
        Debug.Log("Ads Error");
    }

    public void OnUnityAdsDidStart(string placementId)
    {
        Debug.Log("Ads Start");
    }

    public void OnUnityAdsDidFinish(string placementId, ShowResult showResult)
    {
        if (OnAdDone != null)
        {
            AdFinishEventArgs args = new AdFinishEventArgs(placementId, showResult);
            OnAdDone(this, args);
        }
    }

    public void ShowRewardedAd()
    {
        if (isAdFree)
        {
            if (OnAdDone != null)
            {
                AdFinishEventArgs args = new AdFinishEventArgs(videoAdID, ShowResult.Finished);
                OnAdDone(this, args);
            }
            return;
        }
        if (Advertisement.IsReady(rewardVideoAdID))
        {
            Advertisement.Show(rewardVideoAdID);
        }
        else
        {
            Debug.Log("No Ads!");
        }
    }

    private IEnumerator ShowBannerAd_Routine()
    {
        WaitForSeconds updateRate = new WaitForSeconds(0.5f);
        while (!Advertisement.isInitialized)
        {
            yield return updateRate;
        }
        Advertisement.Banner.SetPosition(BannerPosition.BOTTOM_RIGHT);
        Advertisement.Banner.Show(bannerAdID);
    }

    public void ShowBannerAd()
    {
        if (isAdFree)
        {
            return;
        }
        StartCoroutine(ShowBannerAd_Routine());
    }

    public void HideBannerAd()
    {
        if (Advertisement.Banner.isLoaded)
        {
            Advertisement.Banner.Hide();
        }
    }

    public void ShowVideoAd()
    {
        if (isAdFree)
        {
            if (OnAdDone != null)
            {
                AdFinishEventArgs args = new AdFinishEventArgs(rewardVideoAdID, ShowResult.Finished);
                OnAdDone(this, args);
            }
            return;
        }
        if (Advertisement.IsReady(videoAdID))
        {
            Advertisement.Show(videoAdID);
        }
        else
        {
            Debug.Log("No Ads!");
        }
    }

    public void ToggleAd()
    {
        isAdFree = !isAdFree;
        if (isAdFree)
        {
            HideBannerAd();
            toggleAdText.text = "OFF";
        }
        else
        {
            ShowBannerAd();
            toggleAdText.text = "ON";
        }
    }
}
