using UnityEngine;
using GoogleMobileAds.Api;
using UnityEngine.SceneManagement;

public class Banner : MonoBehaviour
{
    //private const string banner = "ca-app-pub-6876201111676185/9547074968";
    private const string banner = "ca-app-pub-3940256099942544/6300978111";
    private const string Bonus = "ca-app-pub-6876201111676185/3528461526";
    private void Awake()
    {
       
        BannerView bannerV = new BannerView(banner, AdSize.Banner,AdPosition.Bottom);
        AdRequest request = new AdRequest.Builder()
            .AddTestDevice("2077ef9a63d2b398840261c8221a0c9b")
            .Build();
        //AdRequest request = new AdRequest.Builder().Build();
        bannerV.LoadAd(request);
        
        
    }
}
