using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlanetInfo : MonoBehaviour
{
    
    [SerializeField]
    private int mineLevels;
    //For create price
    private long priceBuer = 100, priceDyn = 50,priceRobotMiner = 20,priceRocket = 500000,priceFactory = 30,priceLab = 150,priceElevator = 250;
    //Levels Builds
    private int levelFactory, levelLab, levelElevator, levelRocket;
    //Needing level lab for upgrades Lab
    public bool upgradeBagBool, upgradeSpeedBool, upgradeMineBool, upgradeEnergyBool;
    private int Bag = 5, Speed = 10, Mine = 20, Energy = 50;
    private int priceBag = 500, priceSpeed = 1000, priceMine = 2500, priceEnergy = 5000;
   
    private Factory factory;
    private Elevator elevator;
    private Rocket rocket;
    private Lab lab;
    private SAVELOAD saveload;
    Transform canvasScore;
    Score scoreObj;
    Fuel fuelObj;


    //0 - Count MineBlock, 1 - Robots,2-BagUp,3-SpeedUp,4-MineUp,5-EnergyUp;
    public List<List<string>> MINESINFO = new List<List<string>>();

    private void Awake()
    {
        canvasScore = GameObject.Find("CanvasScore").transform;
        scoreObj = canvasScore.Find("Score").GetComponent<Score>();
        fuelObj = canvasScore.Find("Fuel").GetComponent<Fuel>();
        factory = GameObject.Find("Factory").GetComponent<Factory>();
        rocket = GameObject.Find("RocketPlace").GetComponent<Rocket>();
        lab = GameObject.Find("Lab").GetComponent<Lab>();
        elevator = Camera.main.GetComponent<Elevator>();
        saveload = GameObject.Find("SL").GetComponent<SAVELOAD>();
        
    }
    private void Start()
    {
        LoadInfo();
        //Прогрузка цен зданий, на фоне загруженной информации
        Builds[] builds = GameObject.Find("Up/Buttons").GetComponentsInChildren<Builds>();
        foreach (Builds build in builds)
        {
            build.LoadStartInfoBuilds(true);
           
        }
    }

    //Сохранения и загрузка 
    public void PlusMineLevels(int value)
    {
        mineLevels += value;
    }
    public int MineLevels()
    {
        return mineLevels;
    }
    public void RocketStarted(long GC)
    {
        levelRocket = 2;
        saveload.ChangeGC('+', GC);
        rocket.CheckLevel();
        saveload.UpdateScore();
        priceRocket = 500000;
        GameObject.Find("Up/Buttons/btnR").GetComponent<Builds>().LoadStartInfoBuilds(true);
        
    }
    public void UpLvlBuilds(bool Start,string Build,long price)
    {

        switch (Build)
        {
            case "R":
                if(!Start)levelRocket += 1;
                priceRocket = price;
                rocket.CheckLevel();
                break;
            case "E":
                if (!Start) levelElevator += 1;
                priceElevator = price;
                elevator.CheckLevel();
                break;
            case "F":
                if (!Start) levelFactory += 1;
                priceFactory = price;
                factory.CheckLevel();
                break;
            case "L":
                if (!Start) levelLab += 1;
                priceLab = price;
                lab.CheckLevel();
                break;
        }
    }


    //Wached level and upgrades
    public string ShowInfo(string Collection,string Number)
    {
       
        if (Collection == "Levels")
        {
            switch (Number)
            {
                case "R":
                    return levelRocket.ToString();
                case "E":
                    return levelElevator.ToString();
                case "F":
                    return levelFactory.ToString();
                case "L":
                    return levelLab.ToString();

            }
        }
        else if (Collection == "Lab")
        {
            switch (Number)
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
    public long ShowPrice(string Number)
    {

        switch (Number)
        {
            case "R":
                return priceRocket;
            case "E":
                return priceElevator;
            case "F":
                return priceFactory;
            case "L":
                return priceLab;
            case "B":
                return priceBuer;
            case "RB":
                return priceRobotMiner;
            case "D":
                return priceDyn;
            case "UB":
                return priceBag;
            case "US":
                return priceSpeed;
            case "UM":
                return priceMine;
            case "UE":
                return priceEnergy;
        }

        return 404;
    }
    
            
    public void LoadInfo()
    {
        Score score = GameObject.Find("CanvasScore/Score").GetComponent<Score>();
        Fuel fuel = GameObject.Find("CanvasScore/Fuel").GetComponent<Fuel>();

        string[] PlanetParts = saveload.ShowInfo("Planet1").Split('/');
        List<string> PlanetPartList = PlanetParts.ToList();

        score.Plus(long.Parse(PlanetPartList[0]));
        fuel.Plus(long.Parse(PlanetPartList[1]));
        levelRocket = int.Parse(PlanetPartList[2]);
        levelElevator = int.Parse(PlanetPartList[3]); 
        levelFactory = int.Parse(PlanetPartList[4]);
        levelLab = int.Parse(PlanetPartList[5]);

        //Удаление информации о зданиях и счете для прогрузки шахт
        PlanetPartList.RemoveRange(0, 6);
        
        Boer boer = GameObject.Find("Boer").GetComponent<Boer>();
        foreach (string mine in PlanetPartList)
        {
            string[] mineParts = mine.Split('.');
            
            //Создает уровень
            MineExpansion mineExpansion = boer.PlusMine(true).GetComponent<MineExpansion>();
            for (int i = 0;i < int.Parse(mineParts[0]); i++)
            {
                mineExpansion.BuyDyn(true);
            }
            for (int i = 0; i < int.Parse(mineParts[1]); i++)
            {
                mineExpansion.BuyRobot(true);
            }
       
            if(mineParts[2] == "1") mineExpansion.BuyUp("B");
            if(mineParts[3] == "1") mineExpansion.BuyUp("S");
            if(mineParts[4] == "1") mineExpansion.BuyUp("M");
            if(mineParts[5] == "1") mineExpansion.BuyUp("E");
            

        }
    }
    
    
    private void OnDisable()
    {
        saveload.CollectionData("Planet1",GenerateStrPlanets());
    }

    private string GenerateStrPlanets()
    {
        string allparamMinesInfo = "";
        allparamMinesInfo += scoreObj.Value().ToString() + "/";
        allparamMinesInfo += fuelObj.Value().ToString() + "/";
        allparamMinesInfo += levelRocket  + "/";
        allparamMinesInfo += levelElevator + "/";
        allparamMinesInfo += levelFactory + "/";
        allparamMinesInfo += levelLab;
        //Запись списка с шахтами в строку
        foreach (List<string> a in MINESINFO)
        {
            allparamMinesInfo += "/";
            foreach (string b in a)
            {
                allparamMinesInfo += b + ".";
            }
            allparamMinesInfo = allparamMinesInfo.TrimEnd('.');
        }
        return allparamMinesInfo;
    }

}
