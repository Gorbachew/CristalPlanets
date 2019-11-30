using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlanetInfo : MonoBehaviour
{
    
    [SerializeField]
    //For create price
    public const int priceBuer = 100, priceDyn = 10,priceRobotMiner = 20,priceRocket = 10000,priceFactory = 25,priceLab = 1000,priceElevator = 209;
    public const int Bag = 5, Speed = 10, Mine = 20, Energy = 50;
    public const int priceBag = 500, priceSpeed = 1000, priceMine = 2500, priceEnergy = 5000;
    //Levels Builds
    [SerializeField]
    private int levelMine, levelFactory, levelLab, levelElevator, levelRocket;
    //Needing level lab for upgrades Lab
    public bool upgradeBagBool, upgradeSpeedBool, upgradeMineBool, upgradeEnergyBool;



    protected SceneManage SM;

    public void Awake()
    {
        Debug.Log("Awake PlanetInfo");
        SM = GameObject.Find("MainCamera").GetComponent<SceneManage>();
    }


    public void RocketStarted(long GC)
    {
        levelRocket = 2;
        //Прибавляет к глобальным кристалам счет
        SM.SL.ChangeGC('+', GC);
        //Обновляет счет в БД
        if(SM.SL.ShowInfo("Name") != "Anonim") SM.SL.UpdateScore();
        //Обновляет уровень зданий
        SM.btnR.GetComponent<Builds>().LoadInfoBuilds(true);
    }
    public void LvlUpBuilds(string Build)
    {
        switch (Build)
        {
            case "R":
                levelRocket += 1;   
                SM.objRock.GetComponent<Rocket>().CheckLevel();
                break;
            case "E":
                levelElevator += 1;
                SM.objStock.GetComponent<Elevator>().CheckLevel();
                break;
            case "F":
                levelFactory += 1;
                SM.objFac.GetComponent<Factory>().CheckLevel();
                break;
            case "L":
                levelLab += 1;
                SM.objLab.GetComponent<Lab>().CheckLevel();
                break;
            case "M":
                levelMine += 1;
                break;
        }
    }


    //Wached level and upgrades
    public string ShowInfo(string Collection,string Build)
    {
       
        if (Collection == "Levels")
        {
            switch (Build)
            {
                case "R":
                    return levelRocket.ToString();
                case "E":
                    return levelElevator.ToString();
                case "F":
                    return levelFactory.ToString();
                case "L":
                    return levelLab.ToString();
                case "M":
                    return levelMine.ToString();
                    

            }
        }
        else if (Collection == "Lab")
        {
            switch (Build)
            {
                case "B":
                    return Bag.ToString();
                case "S":
                    return Speed.ToString();
                case "M":
                    return Mine.ToString();
                case "E":
                    return Energy.ToString();

            }
        }
        return "Error";
    }
    
    public void LoadInfo()
    {
        //Обнуление переменных
        levelMine = 0; levelFactory = 0; levelLab = 0; levelElevator = 0; levelRocket = 0;
        upgradeBagBool = false; upgradeSpeedBool = false; upgradeMineBool = false; upgradeEnergyBool = false;
        SM.Mines.MINESINFO.Clear();
        //Загрузка новых данных
        Debug.Log(SM.SL.ShowInfo("Planet0"));
        string[] PlanetParts = SM.SL.ShowInfo("Planet0").Split('/');
        List<string> PlanetPartList = PlanetParts.ToList();
        //Обнуляет старый счет и выдает новый
        SM.Score.Load();
        SM.Score.Change("C", "+", long.Parse(PlanetPartList[0]));
        SM.Score.Change("F", "+", long.Parse(PlanetPartList[1]));
        levelRocket = int.Parse(PlanetPartList[2]);
        levelElevator = int.Parse(PlanetPartList[3]);

        string[] factoryParam = PlanetPartList[4].Split('.');
        levelFactory = int.Parse(factoryParam[0]);
        SM.Factory.orderPump = int.Parse(factoryParam[1]);

        levelLab = int.Parse(PlanetPartList[5]);

        //Удаление информации о зданиях и счете для прогрузки шахт
        PlanetPartList.RemoveRange(0, 6);
        //Прогрузка шахт
        foreach (string mine in PlanetPartList)
        {
            List<int> newMine = new List<int>();
            string[] mineParts = mine.Split('.');
            foreach(string part in mineParts)
            {
                newMine.Add(int.Parse(part));
            }

           SM.Mines.MINESINFO.Add(newMine);

        }
        //Прогрузка цен зданий, на фоне загруженной информации
        foreach (Builds build in SM.Builds)
        {
            build.LoadInfoBuilds(true);

        }

        SM.Mines.Load();

        SM.Factory.Load();
        SM.objBoer.GetComponent<Boer>().LoadBoer();
        SM.Rocket.CheckLevel();
        SM.Rocket.Load();
        SM.Elevator.Load();
        SM.objFac.GetComponent<Factory>().CheckLevel();
        SM.objLab.GetComponent<Lab>().CheckLevel();
    } 

    public string GenerateStrPlanets()
    {
        string allparamMinesInfo = "";
        allparamMinesInfo += SM.Score.Value("C").ToString() + "/";
        allparamMinesInfo += SM.Score.Value("F").ToString() + "/";
        allparamMinesInfo += levelRocket + "/";
        allparamMinesInfo += levelElevator + "/";
        allparamMinesInfo += levelFactory + "." + SM.Factory.orderPump.ToString() + "/";
        allparamMinesInfo += levelLab;

        //Запись списка с шахтами в строку
        foreach (List<int> a in SM.Mines.MINESINFO)
        {
            allparamMinesInfo += "/";
            foreach (int b in a)
            {
                allparamMinesInfo += b.ToString() + ".";
            }
            allparamMinesInfo = allparamMinesInfo.TrimEnd('.');
        }
        
        if (allparamMinesInfo.Length > 200)
        {
            Debug.Log("Сгенерировалась инфа " + allparamMinesInfo);
            return allparamMinesInfo;
        }         
        else
        {
            Debug.Log("Какая то ошибка" + allparamMinesInfo);
            return "Error";
        }
        

    }



    private void OnApplicationPause(bool pause)
    {
        string plinfo = GenerateStrPlanets();
        if (plinfo != "Error") SM.SL.CollectionData("Planet0", plinfo);
    }
    private void OnDestroy()
    {
        string plinfo = GenerateStrPlanets();
        if(plinfo != "Error") SM.SL.CollectionData("Planet0", plinfo);
    }

    


}
