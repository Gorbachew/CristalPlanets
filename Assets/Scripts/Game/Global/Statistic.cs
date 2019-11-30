using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Statistic : MonoBehaviour
{
    private Text ScorePlus, FuelNeed;
    private SceneManage SM;
    private float time;

    private void Awake()
    {
        Debug.Log("Awake Statistic");

        SM = GameObject.Find("MainCamera").GetComponent<SceneManage>();
        ScorePlus = gameObject.transform.Find("Canvas/CrystalPlusText").GetComponent<Text>();
        FuelNeed = gameObject.transform.Find("Canvas/FuelNeedText").GetComponent<Text>();

    }

    public void DisplayStatistic()
    {
        SM.Mines.GetAllStatistic();
        ScorePlus.text = SM.Score.ConvertPrice(SM.Mines.minesCrystal);
        FuelNeed.text = SM.Score.ConvertPrice(SM.Mines.minesFuel);

    }

    private void FixedUpdate()
    {
        time -= Time.deltaTime;
        if(time <= 0)
        {
            DisplayStatistic();
            time = 2;
        }
    }


}
