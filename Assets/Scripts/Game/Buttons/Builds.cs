using UnityEngine;
using UnityEngine.UI;

public class Builds : MonoBehaviour
{
    private Text textPrice, textLevel;
    private string nameBuild;
    SceneManage SM;
    [SerializeField]private long price;
    private void Awake()
    {
        Debug.Log("Awake Builds");

        SM = GameObject.Find("MainCamera").GetComponent<SceneManage>();
        textPrice = gameObject.transform.Find("Text").GetComponent<Text>();
        textLevel = gameObject.transform.Find("TextLevel").GetComponent<Text>();



        switch (gameObject.transform.parent.name)
        {
            case "Lab":
                nameBuild = "L";
                break;
            case "RocketPlace":
                nameBuild = "R";
                break;
            case "Stock":
                nameBuild = "E";
                break;
            case "Factory":
                nameBuild = "F";
                break;

        }
    }
  
    public void LoadInfoBuilds(bool Start)
    {
        //Проверяет уровень зданий
        //Если это не старт и позволяет бюджет, то увеличивает уровень и вычитывает цену
        if (SM.Score.CheckPrice(Start,price) && !Start)
        {
            SM.PlanetInfo.LvlUpBuilds(nameBuild);
            SM.Score.Change("C", "-", price);
        }
        //Просчитывает цену на нынешний уровень и выводит в игру
        price = SM.Score.Price(nameBuild);
        textPrice.text = SM.Score.ConvertPrice(price);
        textLevel.text = SM.PlanetInfo.ShowInfo("Levels", nameBuild);

    }

    public void Click()
    {
        LoadInfoBuilds(false);
    }

}
