using UnityEngine;
using UnityEngine.UI;

public class BuyUpgrade : MonoBehaviour
{
    private MineExpansion mineExpansion;
    private void Awake()
    {
        mineExpansion = gameObject.GetComponentInParent<MineExpansion>();
    }

   
    public void PriceUp(string Price)
    {
        gameObject.GetComponentInChildren<Text>().text = Price;

    }

    private void OnMouseUpAsButton()
    {
        gameObject.GetComponentInParent<AudioSource>().Play();
        switch (gameObject.name)
        {
            case "UpgradeBag":
                mineExpansion.BuyUp("B");
             
                break;
            case "UpgradeSpeed":
                mineExpansion.BuyUp("S");
   
                break;
            case "UpgradeMine":
                mineExpansion.BuyUp("M");
            
                break;
            case "UpgradeEnergy":
                mineExpansion.BuyUp("E");
           
                break;
        }
       
    }
}
