using System.Collections.Generic;
using UnityEngine;


public class MineExpansion : MonoBehaviour
{

    private Transform MineBloks,Robots;
    private PlanetInfo planetInfo;
    private GameObject buyRobot, Pipe, NextBlock,upgradeBag, upgradeSpeed, upgradeMine, upgradeEnergy;
    private ParticleSystem particleSystems;
    private Dynamite dynamiteScript;
    private BuyRobot buyRobotScript; 
    private long priceNextBlock;
    private bool checkDyn;
    private int maxRobots = 5;
    private Score score;
    private int numberRobot;
    public List<Robot> RobotsList = new List<Robot>();
    private string robotLevels;
    public int numberLevel;
    public int mineBlocks;
    public bool UpBag, UpSpeed, UpMine, UpEnergy;
    public long prBag, prSpeed, prMine, prEnergy;

    private void Awake()
    {
        score = GameObject.Find("CanvasScore/Score").GetComponent<Score>();
        planetInfo = Camera.main.GetComponent<PlanetInfo>();
        MineBloks = gameObject.transform.Find("MineBlocks");
        Robots = gameObject.transform.Find("Robots");
        NextBlock = MineBloks.Find("MineBlockNext").gameObject;
        Pipe = gameObject.transform.Find("Pipe").gameObject;
        dynamiteScript = NextBlock.GetComponent<Dynamite>();
        buyRobot = gameObject.transform.Find("BuyRobot").gameObject;
        upgradeBag = gameObject.transform.Find("UpgradeBag").gameObject;
        upgradeSpeed = gameObject.transform.Find("UpgradeSpeed").gameObject;
        upgradeMine = gameObject.transform.Find("UpgradeMine").gameObject;
        upgradeEnergy = gameObject.transform.Find("UpgradeEnergy").gameObject;
        buyRobotScript = buyRobot.GetComponent<BuyRobot>();
        particleSystems = NextBlock.GetComponentInChildren<ParticleSystem>();
        //Записывает о себе данные в массив
        planetInfo.PlusMineLevels(1);
        planetInfo.MINESINFO.Add(new List<string>() { mineBlocks.ToString(), "0","0","0","0","0" });

        numberLevel = planetInfo.MineLevels();
    }

    private void Start()
    {   
        NextBlock.SetActive(false);

        buyRobot.SetActive(false);
        Pipe.SetActive(false);
        upgradeBag.SetActive(false);
        upgradeSpeed.SetActive(false);
        upgradeMine.SetActive(false);
        upgradeEnergy.SetActive(false);

        prBag = planetInfo.ShowPrice("UB") * numberLevel;
        prSpeed = planetInfo.ShowPrice("US") * numberLevel;
        prMine = planetInfo.ShowPrice("UM") * numberLevel;
        prEnergy = planetInfo.ShowPrice("UE") * numberLevel;

        //Сообщает уровень роботов
        robotLevels = planetInfo.MINESINFO[numberLevel - 1][1];
        //Выдает цены
        buyRobotScript.Price(score.PriceRobotMiner(true,planetInfo.ShowPrice("RB"),numberLevel,int.Parse(robotLevels)));
        dynamiteScript.PriceNextBlock(score.PriceDyn(true,planetInfo.ShowPrice("D"), numberLevel,mineBlocks));
    }
    private void FixedUpdate()
    {
        CheckProgress();
    }
    public void BuyDyn(bool Load)
    {
        
        if (mineBlocks == 0)
        {
            Instantiate(Resources.Load("Prefab/MineBlockBunch"), MineBloks);
        }
        else
        {
            GameObject mineBlock = Instantiate(Resources.Load("Prefab/MineBlock"), MineBloks) as GameObject;
            mineBlock.transform.SetSiblingIndex(0);
        }
        //Перетаскивает блоки вправо
        NextBlock.transform.SetSiblingIndex(MineBloks.transform.childCount);
        mineBlocks += 1;
        //Увеличивает на 1 кол-во mineBlock
        planetInfo.MINESINFO[numberLevel - 1][0] = (int.Parse(planetInfo.MINESINFO[numberLevel - 1][0]) + 1).ToString();
        //Выдает цену следующему блоку, если load = true то не вычитает деньги за динамит
        dynamiteScript.PriceNextBlock(score.PriceDyn(Load, planetInfo.ShowPrice("D"), numberLevel, mineBlocks));
        
        particleSystems.Play();
        if (MineBloks.transform.childCount >= 9) Destroy(NextBlock.gameObject);           
    }
    public void BuyRobot(bool Load)
    {
        //Увеличивает на 1 кол-во Robots
        planetInfo.MINESINFO[numberLevel - 1][1] = (int.Parse(planetInfo.MINESINFO[numberLevel - 1][1]) + 1).ToString();
        robotLevels = planetInfo.MINESINFO[numberLevel - 1][1];
        //Если роботов больше чем максимум то улучшает робота
        if (int.Parse(robotLevels) > maxRobots)
        {
            RobotsList[numberRobot].LevelUp();
            numberRobot += 1;
            if (numberRobot >= maxRobots) numberRobot = 0;
        }
        else
        {
            GameObject robotObj = Instantiate(Resources.Load("Prefab/RoboMiner"), Robots) as GameObject;
            Robot robot = robotObj.GetComponent<Robot>();
            RobotsList.Add(robot);
        }
        buyRobotScript.Price(score.PriceRobotMiner(Load, planetInfo.ShowPrice("RB"), numberLevel, int.Parse(robotLevels)));  
    }
    public void BuyUp(string Up)
    {
        switch (Up)
        {
            case "B":
                UpBag = true;
                planetInfo.MINESINFO[numberLevel - 1][2] = "1";
                score.Minus(prBag);
                break;
            case "S":
                UpSpeed = true;
                planetInfo.MINESINFO[numberLevel - 1][3] = "1";
                score.Minus(prSpeed);
                break;
            case "M":
                UpMine = true;
                planetInfo.MINESINFO[numberLevel - 1][4] = "1";
                score.Minus(prMine);
                break;
            case "E":
                UpEnergy = true;
                planetInfo.MINESINFO[numberLevel - 1][5] = "1";
                score.Minus(prEnergy);
                break;


        }
    }


    private void CheckProgress()
    {
        //Проверяет можно ли отображать динамит
        if (planetInfo.MineLevels() > numberLevel && !checkDyn)
        {
            //Ок если число шахт больше чем номер шахты (чтобы не загораживал бур)

            if (numberLevel > 1)
            {
                //Проверяет, построена ли шахта сверху
                if (planetInfo.MINESINFO[numberLevel - 2][0] != "0")
                {
                    NextBlock.SetActive(true);
                    checkDyn = true;
                }
            }
            else
            {
                //Первой шахте дает построиться без проверки
                NextBlock.SetActive(true);
                checkDyn = true;
            }

                
        }

        if (mineBlocks > 0)
        {
          buyRobot.SetActive(true);
          Pipe.SetActive(true);
            //Отображает улучшения для роботов если достигнут уровень лаборатории
            if (planetInfo.upgradeBagBool)
            {
                if(!UpBag)upgradeBag.SetActive(true);
                else upgradeBag.SetActive(false);
                upgradeBag.GetComponent<BuyUpgrade>().PriceUp(prBag.ToString());
            }

            if (planetInfo.upgradeSpeedBool)
            {
                if (!UpSpeed) upgradeSpeed.SetActive(true);
                else upgradeSpeed.SetActive(false);
                upgradeSpeed.GetComponent<BuyUpgrade>().PriceUp(prSpeed.ToString());
            }

            if (planetInfo.upgradeMineBool)
            {
                if (!UpMine) upgradeMine.SetActive(true);
                else upgradeMine.SetActive(false);
                upgradeMine.GetComponent<BuyUpgrade>().PriceUp(prMine.ToString());
            }
            if (planetInfo.upgradeEnergyBool)
            {
                if (!UpEnergy) upgradeEnergy.SetActive(true);
                else upgradeEnergy.SetActive(false);
                upgradeEnergy.GetComponent<BuyUpgrade>().PriceUp(prEnergy.ToString());
            }
        }
    }

}
