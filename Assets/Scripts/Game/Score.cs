using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    static string[] namesCicle = { "", "K", "M", "B", "T", "q", "Q", "s", "S", "O" };
    private Text scoreText;
    private Text fuelText;
    
    [SerializeField]
    private long SCORE = 0;
    private long FUEL = 0;


    SceneManage SM;

    private void Awake()
    {
        Debug.Log("Awake Score");
        SM = GameObject.Find("MainCamera").GetComponent<SceneManage>();
        scoreText = gameObject.GetComponent<Text>();
        fuelText = gameObject.transform.parent.Find("Fuel").GetComponent<Text>();
    }
    public void Load()
    {
        SCORE = 0;
        FUEL = 0;
    }
    public void Change(string what,string sign,long value)
    {
        switch (what)
        {
            case "C":
              
                switch (sign)
                {
                    case "+":
                        SCORE += value;
                        break;
                    case "-":
                        SCORE -= value;
                        break;
                }
                scoreText.text = ConvertPrice(SCORE);
                break;
            case "F":
                switch (sign)
                {
                    case "+":
                        FUEL += value;
 
                        break;
                    case "-":
                        FUEL -= value;
                        if (FUEL < 0) FUEL = 0;
                        break;
                }
                fuelText.text = ConvertPrice(FUEL);
                SM.Rocket.FillFuel();
                break;
        }
       SM.objStock.GetComponent<Stock>().StockBunch(SCORE);      
    }

    public long Value(string what)
    {
        switch (what)
        {
            case "C":
                return SCORE;
            case "F":
                return FUEL;
        }
        return 404;
    }
    public long Price(string Build)
    {
        long price = 0;
        float multiply = 0;
        int levels = 0;
        switch (Build)
        {
            case "L":
                price = PlanetInfo.priceLab;
                multiply = 15f;
                levels = int.Parse(SM.PlanetInfo.ShowInfo("Levels", Build));
                break;
            case "E":
                price = PlanetInfo.priceElevator;
                levels = int.Parse(SM.PlanetInfo.ShowInfo("Levels", Build));
                if(levels >= 15) multiply = 50f;
                else if(levels >= 10) multiply = 30f;
                else if(levels >= 5) multiply = 20f;
                else if(levels >= 1) multiply = 2f;
                break;
            case "F":
                price = PlanetInfo.priceFactory;
                multiply = 7f;
                levels = int.Parse(SM.PlanetInfo.ShowInfo("Levels", Build));
                break;
            case "R":
                price = PlanetInfo.priceRocket;
                
                levels = int.Parse(SM.PlanetInfo.ShowInfo("Levels", Build));
                if (levels > 2)
                {
                    multiply = 0.3f;
                }
                else multiply = 1.2f;

                break;
            case "B":
                price = PlanetInfo.priceBuer;
                levels = int.Parse(SM.PlanetInfo.ShowInfo("Levels", "M"));
                if (levels >= 15) multiply = 50f;
                else if (levels >= 10) multiply = 30f;
                else if (levels >= 5) multiply = 15f;
                else if (levels >= 3) multiply = 7f;
                else if (levels >= 1) multiply = 2f;
                
                break;
        }
        //Чтобы на старте прошел хотя бы один цикл
        //levels++;

        price = (long)(price * levels * multiply);
        return price;
    }
   
    public long PriceMine(string Thing,int indexMine)
    {
        //Это можно написать лучше
        long price = 0;
        int Amount = 0;
        //Индекс и Кол-во + 1 чтобы избежать умножений на 0 в цикле
        switch (Thing)
        {
            case "D":
                price = PlanetInfo.priceDyn;
                Amount = SM.Mines.MINESINFO[indexMine][0];
                break;
            case "R":
                price = PlanetInfo.priceRobotMiner;
                Amount = SM.Mines.MINESINFO[indexMine][1];
                break;
        }
        price = (long)((indexMine + 1) * price) * (Amount + 1);
        //Чтобы побольше были цены
        if (indexMine + 1 >= 15) price *= 100;
        else if (indexMine + 1 >= 10) price *= 50;
        else if (indexMine + 1 >= 5) price *= 20;
        else if (indexMine + 1 >= 2) price *= 5;
        return price;
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
    public bool CheckPrice(bool Start,long price)
    {
        if (!Start)
        {
            if (price > SCORE)
            {
                SM.ErrorSource.Play();
                if (gameObject.activeSelf) StartCoroutine(ErrorCristal());
                if (SCORE <= long.Parse(SM.PlanetInfo.ShowInfo("Levels","F")) && FUEL <= 0) SM.table.Open("BonusDead");
                return false;
            }
            else
            {
                SM.BuySourse.Play();
                return true;
            }
        }
        return false;
    }
    public bool CheckFuel(long value)
    {
        Debug.Log(value);
        if (value > FUEL)
        {
            SM.ErrorSource.Play();
            if (gameObject.activeSelf) StartCoroutine(ErrorFuel());
            return false;
        }
        else
        {
            SM.BuySourse.Play();
            return true;
        }
    }


    IEnumerator ErrorCristal()
    {
        scoreText.color = Color.red;
        yield return new WaitForSeconds(0.2f);
        scoreText.color = Color.yellow;
    }
    IEnumerator ErrorFuel()
    {
        fuelText.color = Color.red;
        yield return new WaitForSeconds(0.2f);
        fuelText.color = Color.yellow;
    }
}
