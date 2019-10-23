using System;

using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    static string[] namesCicle = { "", "K", "M", "B", "T", "q", "Q", "s", "S", "O" };
    private Text scoreText;
    private PlanetInfo planetInfo;
    private Stock stock;
    [SerializeField]
    private long SCORE = 0;
    

    //Using in method
    private long price, level;

    private void Awake()
    {
        scoreText = gameObject.GetComponent<Text>();
        planetInfo = Camera.main.GetComponent<PlanetInfo>();
        stock = GameObject.Find("Up/Stock").GetComponent<Stock>();
    }

    public void Plus(long value)
    {

        SCORE += value;
        scoreText.text = ConvertPrice(SCORE);
        stock.StockBunch();
    }
    public void Minus(long value)
    {
        SCORE -= value;
        scoreText.text = ConvertPrice(SCORE);
        stock.StockBunch();
    }
    public long Value()
    {
        return SCORE;
    }
    public string PriceBuer(bool Start, long priceBuer,int mineLevels)
    {
        long price = priceBuer;
        if (!Start) Minus(price);
        for (int i = 0; i < mineLevels; i++)
        {
            price = (long)(price * 1.8f);
        }
        return ConvertPrice(price);
    }
    public string PriceDyn(bool Start,long priceDyn,int levelMine, int Blocks)
    {
        long price = priceDyn;
        int mineBlock = Blocks;
        if (!Start) Minus(price);
        for (int i = -1; i < mineBlock; i++)
        {
            price = price + (levelMine * 20 * (mineBlock + 1));

        }
        return ConvertPrice(price);
    }
    public string PriceRobotMiner(bool Start,long priceRobotMiner, int levelMine,int Robots)
    {
        long price = priceRobotMiner;
        int countRobots = Robots;
        for (int i = 0; i < countRobots + 1; i++)
        {
            price = (long)((price + (levelMine * 10)) * 1.1);
        }
        if (!Start)Minus(price);
     
        return ConvertPrice(price);
    }

    public string ConvertPrice(long value)
    {
        string price = value.ToString();
        int cycles = 0;
        if (price.Length > 4)
        {
            while (price.Length > 6)
            {
                cycles += 1;
                price = price.Remove(price.Length - 3);
            }
            string balance = price.Substring(price.Length - 3);
            price = price.Remove(price.Length - 3) + "." + balance.Remove(balance.Length - 2) + namesCicle[cycles + 1];
        }
        return price;
    }

    public string PriceBuilds(bool Start, string Build, float Multiply)
    {
        
        price = planetInfo.ShowPrice(Build);
        level = int.Parse(planetInfo.ShowInfo("Levels", Build));
        if (!Start)
        {
            Minus(price);
            
            price = (long)Math.Ceiling(price * Multiply);
        }
        else
        {
            //Если только вкл, то просчитывает от оригинальной цены
            for (int i = 1; i < level; i++)
            {
                price = (long)Math.Ceiling(price * Multiply);
            }
        }
        planetInfo.UpLvlBuilds(Start, Build, price);
        return ConvertPrice(price);
    }
}
