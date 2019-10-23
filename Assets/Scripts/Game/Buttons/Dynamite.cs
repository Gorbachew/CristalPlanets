using UnityEngine;
using UnityEngine.UI;

public class Dynamite: MonoBehaviour
{
    private MineExpansion mineExpansion;
    private void Start()
    {
        mineExpansion = gameObject.GetComponentInParent<MineExpansion>();
    }
    public void PriceNextBlock(string Price)
    {
        gameObject.GetComponentInChildren<Text>().text = Price;
        
    }
    private void OnMouseUpAsButton()
    {
        gameObject.GetComponent<AudioSource>().Play();
        mineExpansion.BuyDyn(false);    
    }
}
