using UnityEngine;
using UnityEngine.SceneManagement;
using GoogleMobileAds.Api;
public class OpenPlanet : MonoBehaviour
{
    private const string loadPlanets = "ca-app-pub-6876201111676185/2790094921";
    private bool landing;

    private void Awake()
    {
   
    }
    private void OnMouseUpAsButton()
    {
        landing = true;
        gameObject.GetComponentInParent<AudioSource>().Play();
    }
    private void FixedUpdate()
    {
        if (landing) Landing(gameObject);   
    }
    private void Landing(GameObject planet)
    {

        InterstitialAd ad = new InterstitialAd(loadPlanets);
        //AdRequest request = new AdRequest.Builder().Build();
        AdRequest request = new AdRequest.Builder().AddTestDevice(AdRequest.TestDeviceSimulator).AddTestDevice("CDECCA09CEF9D953").Build();
        ad.LoadAd(request);
        if (ad.IsLoaded())
        {
            ad.Show();
        }
        Camera.main.transform.position = Vector3.MoveTowards(Camera.main.transform.position,
            new Vector3(planet.transform.position.x, planet.transform.position.y, planet.transform.position.z - 10),

            // new Vector3(Camera.main.transform.position.x,Camera.main.transform.position.y, .z),
            150.0f * Time.deltaTime);

        if(Camera.main.transform.position.z >= planet.transform.position.z - 11)
        {
            SceneManager.LoadScene("Planet0");
        }
        
    }

}
