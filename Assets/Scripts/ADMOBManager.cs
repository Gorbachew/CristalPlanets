
using UnityEngine;
using GoogleMobileAds.Api;
using System;

public class ADMOBManager : MonoBehaviour
{

    private BannerView bannerView;
    private InterstitialAd interstitial;
    private int IntersPower = 0;
    private RewardedAd rewardedAd;
    private int stateBonus; 
    private AdRequest request;

    /*Реклама
    private const string banner = "ca-app-pub-3940256099942544/6300978111";
    private const string adUnitId = "ca-app-pub-3940256099942544/1033173712";
    private const string adRevardId = "ca-app-pub-3940256099942544/5224354917";
    */
    private const string banner = "ca-app-pub-6876201111676185/9547074968";
    private const string adUnitId = "ca-app-pub-6876201111676185/2790094921";
    private const string adRevardId = "ca-app-pub-6876201111676185/3528461526";

    SceneManage SM;

    private void Start()
    {
        SM = GameObject.Find("MainCamera").GetComponent<SceneManage>();
        MobileAds.Initialize(initStatus => { });
        // Called when the user should be rewarded for interacting with the ad.


        Banner();

        Interstial();

        Video();

        // Called when the user should be rewarded for interacting with the ad.
        this.rewardedAd.OnUserEarnedReward += HandleUserEarnedReward;
    }


    private void Banner()
    {
        // Create an empty ad request.
        this.request = new AdRequest.Builder().Build();
        this.bannerView = new BannerView(banner, AdSize.Banner, AdPosition.Bottom);
        // Create an empty ad request.
        request = new AdRequest.Builder().Build();

        // Load the banner with the request.
        this.bannerView.LoadAd(request);
    }

    private void Interstial()
    {
        this.interstitial = new InterstitialAd(adUnitId);
        // Load the interstitial with the request.
        this.interstitial.LoadAd(request);
    }

    private void Video()
    {
        this.rewardedAd = new RewardedAd(adRevardId);
   
        // Create an empty ad request.
        AdRequest request = new AdRequest.Builder().Build();
        // Load the rewarded ad with the request.
        this.rewardedAd.LoadAd(request);
    }

    public void VideoShow(int stateBonus)
    {
        if (this.rewardedAd.IsLoaded())
        {
            this.rewardedAd.Show();
            StartCoroutine(SM.table.BonusOn(stateBonus, 30));
        }
        Video();
    }
    //Проверяет, загружено ли видео
    public bool VideoLoaded()
    {
        if (this.rewardedAd.IsLoaded())
        {
            return true;
        }
        return false;
    }
    public void HandleUserEarnedReward(object sender, Reward args)
    {
        string type = args.Type;
        double amount = args.Amount;
        Debug.Log("Выдать награду ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++ +=== 12123123");
        
    }
    public void InterstialShow()
    {
        IntersPower++;

        if (this.interstitial.IsLoaded() && IntersPower >= 10)
        {
            this.interstitial.Show();
            IntersPower = 0;
        }
        Interstial();
    }
}

