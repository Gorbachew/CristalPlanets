using UnityEngine;
using UnityEngine.UI;
using GoogleMobileAds.Api;

public class Table : MonoBehaviour
{
    GameObject table;
    Text text;

    

    private void Awake()
    {
        table = GameObject.Find("Table");
        text = table.GetComponentInChildren<Text>();
        table.SetActive(false);
    }

    public void BonusFactory()
    {
        table.SetActive(true);
        text.text = "The factory works for free";
    }

    public void Yes()
    {
        Debug.Log("OpenVideo");
        table.SetActive(false);
    }
    public void No()
    {
        table.SetActive(false);
    }
}
