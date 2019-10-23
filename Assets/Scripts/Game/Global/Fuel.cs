using UnityEngine;
using UnityEngine.UI;

public class Fuel : MonoBehaviour
{
    private Text fuelText;
    private long FUEL = 0;
    private Score score;
    private void Awake()
    {
        score = GameObject.Find("CanvasScore/Score").GetComponent<Score>();
        fuelText = gameObject.GetComponent<Text>();
    }

    public void Plus(long value)
    {
        FUEL += value;
        fuelText.text = score.ConvertPrice(FUEL);
    }
    public void Minus(long value)
    {
        FUEL -= value;
        if(FUEL < 0)
        {
            FUEL = 0;
        }
        fuelText.text = score.ConvertPrice(FUEL);
    }
    public long Value()
    {
        return FUEL;
    }
}
