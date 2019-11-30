using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Bunch : MonoBehaviour
{
    ParticleSystem particleSystems;
    Image bonus;
    
    private int BonusPower = 0,needBonusPower,bonusType;
 

    SceneManage SM;
    //Table table;
    // Start is called before the first frame update
    private void Awake()
    {
        Debug.Log("Awake Bunch");

        SM = GameObject.Find("MainCamera").GetComponent<SceneManage>();
        bonus = gameObject.transform.Find("Bonus").GetComponentInChildren<Image>();
        particleSystems = gameObject.GetComponentInChildren<ParticleSystem>();
        bonus.gameObject.SetActive(false);
        //Рандом отвечающий за сколько раз надо прийти роботам к куче, чтобы сгенирировался бонус
        needBonusPower = Random.Range(1, 400);
    }

    
    public void goDust()
    {
        
        particleSystems.Play();
        //Здеся Звук
        SM.MineSource.Play();
    }


    
    public void Bonus()
    {
        
        if (!bonus.gameObject.activeSelf && SM.Planet.activeSelf)
        {
            BonusPower += 1;
            //Если мы на планете и бонус не активный. Прибавляет значение. Если значение больше или равно делает бонус
            if (BonusPower >= needBonusPower && SM.admob.VideoLoaded())
            {
                SM.BonusSourse.Play();
                BonusPower = 0;
                needBonusPower = Random.Range(1, 400);
                
                bonusType = Random.Range(0, 3);
                switch (bonusType)
                {
                    case 0:
                        bonus.sprite = Resources.LoadAll("Sprites/Textures2")[23] as Sprite;
                        break;
                    case 1:
                        bonus.sprite = Resources.LoadAll("Sprites/Textures2")[24] as Sprite;
                        break;
                    case 2:
                        bonus.sprite = Resources.LoadAll("Sprites/Textures2")[25] as Sprite;
                        break;
                }


                bonus.gameObject.SetActive(true);
                StartCoroutine(DestroyBonus());
            }
            
        }   
    }
   
    public void ClickBonus()
    {
   
        SM.BtnSource.Play();
        switch (bonusType)
        {
            case 0:
                SM.table.Open("BonusRobot");
                break;
            case 1:
                SM.table.Open("BonusFactory");
                break;
            case 2:
                SM.table.Open("BonusCrystal");
                break;
        }
        bonus.gameObject.SetActive(false);
        SM.objTable.SetActive(true);
    }
    
    IEnumerator DestroyBonus()
    {
        yield return new WaitForSeconds(30);
        bonus.gameObject.SetActive(false);
        BonusPower = 0;
    }

    


}
