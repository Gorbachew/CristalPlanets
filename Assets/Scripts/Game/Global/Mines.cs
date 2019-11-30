using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mines : MonoBehaviour
{
    [SerializeField]
    public MineExpansion[] mineExpansions;
    //0 - Count MineBlock, 1 - Robots,2-BagUp,3-SpeedUp,4-MineUp,5-EnergyUp;
    public List<List<int>> MINESINFO = new List<List<int>>();
    // Start is called before the first frame update
    private SceneManage SM;
    public int minesCrystal, minesFuel;

    //Загружает уровни шахт

    private void Awake()
    {
        Debug.Log("Awake Mines");
        SM = GameObject.Find("MainCamera").GetComponent<SceneManage>();
        mineExpansions = GetComponentsInChildren<MineExpansion>();
        foreach (MineExpansion mine in mineExpansions)
        {
            mine.gameObject.SetActive(false);
          //  mine.Connect();
        }
    }

    public void Load()
    {
        foreach (MineExpansion mine in mineExpansions) mine.gameObject.SetActive(false);
        foreach (List<int> mine in MINESINFO)
        {
            if (mine[0] >= 1)
            {
                BuyLevelMine(true);
            }
           
        }
        
    }
    //Бурение новой шахты
    public void BuyLevelMine(bool Start)
    {  
            //Получаем сущность уровня по id 
            MineExpansion mine = mineExpansions[int.Parse(SM.PlanetInfo.ShowInfo("Levels", "M"))];
            mine.gameObject.SetActive(true);
            //Дает 1 блок за NextBlock, чтобы игра запомнила, что шахта куплена
            if (!Start) MINESINFO[mine.indexMine][0] += 1;

            SM.PlanetInfo.LvlUpBuilds("M");//Повышается общий уровень шахт
            CheckMines("B");
            CheckMines("R");
            CheckMines("U");
    }
    //Проверяет прогресс шахт
    public void CheckMines(string State)
    {
      
        foreach(MineExpansion mine in mineExpansions)
        {
            if (mine.gameObject.activeSelf)
            {
                switch (State)
                {
                    case "B":
                        mine.CheckBlocks();
                        break;
                    case "R":
                        mine.CheckRobots("R");
                        break;
                    case "U":
                        mine.CheckRobots("U");
                        if (SM.PlanetInfo.upgradeBagBool && MINESINFO[mine.indexMine][2] == 0) mine.CheckUpgrade("B");
                        if (SM.PlanetInfo.upgradeSpeedBool && MINESINFO[mine.indexMine][3] == 0) mine.CheckUpgrade("S");
                        if (SM.PlanetInfo.upgradeMineBool && MINESINFO[mine.indexMine][4] == 0) mine.CheckUpgrade("M");
                        if (SM.PlanetInfo.upgradeEnergyBool && MINESINFO[mine.indexMine][5] == 0) mine.CheckUpgrade("E");
                        break;

                }  
            } 
        }
    }

    public void GetAllStatistic()
    {
        int value = 0;
        int fuel = 0;
        foreach (MineExpansion mine in mineExpansions)
        {
            mine.GetStatistic();
            value += mine.mineCristals;
            fuel += mine.mineFuel;
        }
        minesCrystal = value;
        minesFuel = fuel;
    }


    //Выдает значение последней активной шахты
    public GameObject LastMine()
    {
        int indexlastmine = int.Parse(SM.PlanetInfo.ShowInfo("Levels", "M")) - 1;
        if (indexlastmine < 0) indexlastmine = 0;
        return mineExpansions[indexlastmine].gameObject;
    }


}
