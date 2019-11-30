using Firebase.Database;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
//using GoogleMobileAds.Api;

public class Table : MonoBehaviour
{
    private Text text,textRating,textRating2;
    private Button btnRating;
    private SceneManage SM;
    private int StateBonus;
    public void Awake()
    {
        Debug.Log("Awake Table");

        SM = gameObject.GetComponent<SceneManage>();
        
    }

    private void Start()
    {
        text = SM.objTable.GetComponentInChildren<Text>();
        textRating = SM.objRatingTableCont.transform.Find("Text").GetComponent<Text>();
        textRating2 = SM.objRatingTableCont.transform.Find("Text2").GetComponent<Text>();
        btnRating = SM.objRatingTableCont.transform.Find("Yes").GetComponent<Button>();
        OffContents();
    }


    public void No()
    {
        OffContents();
    }

    
    
    //Кнопка соглашения
    public void Yes(string What)
    {
        switch (What)
        {
            case "Bonus":
                SM.admob.VideoShow(StateBonus);
          
                break;
            case "Reg":
                if (long.Parse(SM.SL.ShowInfo("GC")) >= 1000)
                {
                    SM.BtnSource.Play();
                    StartCoroutine(SM.addUser.WaitCheck());
                    SM.SL.ChangeGC('-', 1000);
                }
                else SM.ErrorSource.Play();
                break;
            case "Ref":             
                SM.SL.RefreshPlayer(); 
                break;
            case "RollBack":
                SM.SL.RollBackPlayer();
                break;
        }
        SM.BtnSource.Play();
        OffContents();
    }
    public void Open(string What)
    {
        if (!SM.objTable.activeSelf)
        {
            OffContents();
            SM.objTable.gameObject.SetActive(true);
            //Отключает другие слушатели для кнопки
            btnRating.onClick.RemoveAllListeners();
            switch (What)
            {
                case "Reg":
                    SM.objRegTableCont.SetActive(true);
                    break;
                case "Ref":
                    SM.objRatingTableCont.SetActive(true);
                    textRating.text = "Restart progress?";
                    textRating2.gameObject.SetActive(true);
                    btnRating.onClick.AddListener(delegate () { Yes("Ref"); });
                    break;
                case "RollBack":
                    SM.objRatingTableCont.SetActive(true);
                    textRating.text = "RollBack Save?";
                    textRating2.gameObject.SetActive(false);
                    btnRating.onClick.AddListener(delegate () { Yes("RollBack"); });
                    break;
                case "BonusRobot":
                    SM.objBonusTableCont.SetActive(true);
                    text.text = "Robots work faster";
                    StateBonus = 3;
                    break;
                case "BonusFactory":
                    SM.objBonusTableCont.SetActive(true);
                    text.text = "The factory works for better";
                    StateBonus = 2;
                    break;
                case "BonusCrystal":
                    SM.objBonusTableCont.SetActive(true);
                    text.text = "Robots mined more crystals";
                    StateBonus = 1;
                    break;
                case "BonusDead":
                    SM.objBonusTableCont.SetActive(true);
                    text.text = "Get crystal and fuel for ad";
                    StateBonus = 4;
                    break;

            }
        }
        else OffContents();
        
    }

    //Откатить сейв файл

    public IEnumerator BonusOn(int bonusType,int Second)
    {
        switch (bonusType)
        {
            case 1:
                SM.objBonusC.SetActive(true);
                break;
            case 2:
                SM.objBonusF.SetActive(true);
                SM.objFac.GetComponent<Factory>().CheckOrderPump();
                break;
            case 3:
                SM.objBonusR.SetActive(true);
                break;
            case 4:
                SM.Score.Change("C", "+", 100);
                SM.Score.Change("F", "+", 30);
                break;
        }
        yield return new WaitForSeconds(Second);
        switch (bonusType)
        {
            case 1:
                SM.objBonusC.SetActive(false);
                break;
            case 2:
                SM.objBonusF.SetActive(false);
                break;
            case 3:
                SM.objBonusR.SetActive(false);
                break;
        }
    }
    private void OffContents()
    {
        SM.objRegTableCont.SetActive(false);
        SM.objRatingTableCont.SetActive(false);
        SM.objBonusTableCont.SetActive(false);
        SM.objTable.SetActive(false);
    }
}
