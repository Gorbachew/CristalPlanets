using UnityEngine;
using UnityEngine.UI;

public class BuyRobot : MonoBehaviour
{
    private MineExpansion mineExpansion;

    private void Start()
    {
        mineExpansion = gameObject.GetComponentInParent<MineExpansion>();
    }
    public void Price(string Price)
    {
        gameObject.GetComponentInChildren<Text>().text = Price;
    }


    private void OnMouseUpAsButton()
    {
        gameObject.GetComponentInParent<AudioSource>().Play();
        mineExpansion.BuyRobot(false);
    }

}
