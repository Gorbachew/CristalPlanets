using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MineExpansion : MonoBehaviour
{

    private Transform MineBlocks, Robots;
    [SerializeField]
    private Robot[] robots;
    private ParticleSystem psBoom;
    [SerializeField]
    private Mines Mines;
    public int indexMine, mineCristals,mineFuel;
    [SerializeField]
    List<GameObject> blocks = new List<GameObject>();

    [SerializeField] private long[] prices = new long[2] {0,0};
    Text dynPrice, pBuyRobot, pUpBag, pUpSpeed, pUpMine, pUpEnergy;
    SceneManage SM;
    public GameObject Pipe, buyRobot, upgradeBag, upgradeSpeed, upgradeMine, upgradeEnergy;
    public GameObject nextBlock;

    private void Awake()
    {

        Debug.Log("Awake MineExpansions");
        SM = GameObject.Find("MainCamera").GetComponent<SceneManage>();

        MineBlocks = gameObject.transform.Find("MineBlocks");
        Robots = gameObject.transform.Find("Robots");
        Mines = gameObject.transform.parent.GetComponent<Mines>();
        indexMine = transform.GetSiblingIndex();

        dynPrice = gameObject.transform.Find("MineBlocks/MineBlockNext").GetComponentInChildren<Text>();
        pBuyRobot = gameObject.transform.Find("BuyRobot").GetComponentInChildren<Text>();
        pUpBag = gameObject.transform.Find("BuyBag").GetComponentInChildren<Text>();
        pUpSpeed = gameObject.transform.Find("BuySpeed").GetComponentInChildren<Text>();
        pUpMine = gameObject.transform.Find("BuyMine").GetComponentInChildren<Text>();
        pUpEnergy = gameObject.transform.Find("BuyEnergy").GetComponentInChildren<Text>();

        Pipe = gameObject.transform.Find("Pipe").gameObject;
        buyRobot = gameObject.transform.Find("BuyRobot").gameObject;
        upgradeBag = gameObject.transform.Find("BuyBag").gameObject;
        upgradeSpeed = gameObject.transform.Find("BuySpeed").gameObject;
        upgradeMine = gameObject.transform.Find("BuyMine").gameObject;
        upgradeEnergy = gameObject.transform.Find("BuyEnergy").gameObject;
        

        buyRobot.SetActive(false);
        Pipe.SetActive(false);
        upgradeBag.SetActive(false);
        upgradeSpeed.SetActive(false);
        upgradeMine.SetActive(false);
        upgradeEnergy.SetActive(false);

        //Роботы
        robots = Robots.GetComponentsInChildren<Robot>();

        LoadBlocks();
        //nextBlock имеет ссылку только после LoadBlocks()
        psBoom = nextBlock.GetComponentInChildren<ParticleSystem>();

    }
   
    private void LoadBlocks()
    {
        //Получение ссылок на роботов и блоков сначала игры
        //Блоки
        Transform[] allComponents = MineBlocks.GetComponentsInChildren<Transform>();

        foreach (Transform components in allComponents)
        {
            if (components.name == "MineBlock" || components.name == "MineBlockBunch")
            {
                blocks.Add(components.gameObject);
                components.gameObject.SetActive(false);
            }
            else if (components.name == "MineBlockNext")
            {
                nextBlock = components.gameObject;
                components.gameObject.SetActive(false);
            }
        }
        
        foreach (Robot robot in robots)
        {

            robot.gameObject.SetActive(false);
        }

    }


    public void BtnClick(string Btn)
    {
        //SM.BuySourse.Play();
        int index = indexMine + 1;
        long price = 0;
        switch (Btn)
        {
            case "D":

                if (SM.Score.CheckPrice(false,prices[0]))
                {
                    SM.Score.Change("C", "-", prices[0]);
                    SM.DynSource.Play();
                    Mines.MINESINFO[indexMine][0] += 1;
                    psBoom.Play();
                    // dynPrice.text = SM.Score.ConvertPrice(price);
                    SM.Mines.CheckMines("B");
                    SM.Mines.CheckMines("U");
                }
                break;
            case "R":

                if (SM.Score.CheckPrice(false, prices[1]))
                {
                    SM.Score.Change("C", "-", prices[1]);
                    SM.Mines.MINESINFO[indexMine][1] += 1;
                    CheckRobots("R");
                
                }
                break;
            case "B":
                price = index * PlanetInfo.priceBag;
                if (SM.Score.CheckPrice(false, price))
                {
                    SM.Mines.MINESINFO[indexMine][2] += 1;
                    CheckRobots("U");
                    SM.Score.Change("C", "-", price);
                }
                break;
            case "S":
                price = index * PlanetInfo.priceSpeed;
                if (SM.Score.CheckPrice(false, price))
                {
                    SM.Mines.MINESINFO[indexMine][3] += 1;
                    CheckRobots("U");
                    SM.Score.Change("C", "-", price);
                }
                break;
            case "M":
                price = index * PlanetInfo.priceMine;
                if (SM.Score.CheckPrice(false, price))
                {
                    SM.Mines.MINESINFO[indexMine][4] += 1;
                    CheckRobots("U");
                    SM.Score.Change("C", "-", price);
                }
                break;
            case "E":
                price = index * PlanetInfo.priceEnergy;
                if (SM.Score.CheckPrice(false, price))
                {
                    SM.Mines.MINESINFO[indexMine][5] += 1;
                    CheckRobots("U");
                    SM.Score.Change("C", "-", price);
                }
                break;
        }

    }
    public void CheckBlocks()
    {

        //Выдает динамиту цену
        prices[0] = SM.Score.PriceMine("D", indexMine);
        dynPrice.text = SM.Score.ConvertPrice(prices[0]);
        
        //Проверяет на наличие блоков в общем массиве и выставляет их в нужном порядке
        int blockCount = Mines.MINESINFO[indexMine][0] - 1;
        if (blockCount > 0)
        {
            buyRobot.SetActive(true);
            Pipe.SetActive(true);
        }
        foreach (GameObject block in blocks)
        {
            if (blockCount > 0)
            {
                blockCount--;
                block.SetActive(true);
                block.transform.SetSiblingIndex(0);
          
            }
            else block.SetActive(false);
        }
        
        //Проверка на возможность взрыва динамита
        if (!nextBlock.activeSelf)
        {

            int topmine = indexMine - 1;
            if (indexMine > 0)
            {
                if (Mines.MINESINFO[topmine][0] > 1) nextBlock.SetActive(true);
            }
            else if (indexMine == 0) nextBlock.SetActive(true);
        }

        //Если блоки заканчиваются, то динамит пропадает
        if (nextBlock.transform.GetSiblingIndex() >= 8)
        {
            nextBlock.SetActive(false);
        }
    }
    public void CheckRobots(string value)
    {
        switch (value)
        {
            //Robot
            case "R":
                //Высчитывает цену
                prices[1] = SM.Score.PriceMine("R", indexMine);
                pBuyRobot.text = SM.Score.ConvertPrice(prices[1]);
                //Проверка роботов
                int robotCount = Mines.MINESINFO[indexMine][1];
                int[] levels = new int[5] { 0, 0, 0, 0, 0 };
                //По порядку дробит robotCount на уровни каждому роботу
                for (int i = 0; i < robotCount; i++)
                {
                    if (levels[0] == levels[4]) levels[0]++;
                    else if (levels[0] > levels[1]) levels[1]++;
                    else if (levels[1] > levels[2]) levels[2]++;
                    else if (levels[2] > levels[3]) levels[3]++;
                    else if (levels[3] > levels[4]) levels[4]++;
                }
                //Причисляет каждому роботу свой уровень
                int robotNumber = 0;
                foreach (Robot robot in robots)
                {

                    if (levels[robotNumber] > 0) robot.gameObject.SetActive(true);
                    else robot.gameObject.SetActive(false);
                    if (robot.gameObject.activeSelf)
                    {
                        robot.LevelUp(levels[robotNumber]);
                        robot.Load();
                    }
                    
                    robotNumber++;
                }
                
                
                break;
            //Upgrade
            case "U":
                foreach (Robot robot in robots)
                {
                    robot.CheckUp(indexMine);
                    if (SM.Mines.MINESINFO[indexMine][2] == 1) upgradeBag.SetActive(false);
                    if (SM.Mines.MINESINFO[indexMine][3] == 1) upgradeSpeed.SetActive(false);
                    if (SM.Mines.MINESINFO[indexMine][4] == 1) upgradeMine.SetActive(false);
                    if (SM.Mines.MINESINFO[indexMine][5] == 1) upgradeEnergy.SetActive(false);

                }
                break;

        }

    }

    public void CheckUpgrade(string value)
    {
        //Проверяет, отображать ли улучшения если достигнут уровень лаборатории
        int blockCount = Mines.MINESINFO[indexMine][0];
        if (blockCount > 1)
        {
            int index = indexMine + 1;
            switch (value)
            {
                case "B":
                    upgradeBag.SetActive(true);
                    pUpBag.text = SM.Score.ConvertPrice(PlanetInfo.priceBag * index);
                    break;
                case "S":
                    upgradeSpeed.SetActive(true);
                    pUpSpeed.text = SM.Score.ConvertPrice(PlanetInfo.priceSpeed * index);
                    break;
                case "M":
                    upgradeMine.SetActive(true);
                    pUpMine.text = SM.Score.ConvertPrice(PlanetInfo.priceMine * index);
                    break;
                case "E":
                    upgradeEnergy.SetActive(true);
                    pUpEnergy.text = SM.Score.ConvertPrice(PlanetInfo.priceEnergy * index);
                    break;
            }
        }
    }

    public void GetStatistic()
    {
        int value = 0;
        int fuel = 0;
        foreach(Robot robot in robots)
        {
            if (robot.gameObject.activeSelf)
            {
                value += robot.valueBag;
                fuel += robot.fuelNeed;
            }
            
        }
        mineCristals = value;
        mineFuel = fuel;
    }
}
