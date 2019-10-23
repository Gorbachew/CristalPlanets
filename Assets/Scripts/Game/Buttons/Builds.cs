using UnityEngine;
using UnityEngine.UI;

public class Builds : MonoBehaviour
{
    private Transform Canvas;
    private PlanetInfo planetInfo;
    private Text textPrice,textLevel;
    private string nameBuild;
    private Score score;

    private void Awake()
    {
        score = GameObject.Find("CanvasScore/Score").GetComponent<Score>();
        planetInfo = Camera.main.GetComponent<PlanetInfo>();
        Canvas = gameObject.transform.Find("TextPlace/Canvas");
        textPrice = Canvas.Find("Text").GetComponent<Text>();
        textLevel = Canvas.Find("TextLevel").GetComponent<Text>();
        switch (gameObject.name)
        {
            case "btnL":
                nameBuild = "L";
                break;
            case "btnR":
                nameBuild = "R";
                break;
            case "btnE":
                nameBuild = "E";
                break;
            case "btnF":
                nameBuild = "F";
                break;
        }
    }
    
    
    private void OnMouseUpAsButton()
    {
        gameObject.GetComponentInParent<AudioSource>().Play();
        LoadStartInfoBuilds(false);
    }

    public void LoadStartInfoBuilds(bool Start)
    {
       
        string price = score.PriceBuilds(Start, nameBuild, 1.1f);
        
        textPrice.text = price;
        textLevel.text = planetInfo.ShowInfo("Levels", nameBuild);
    }
}
