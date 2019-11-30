using System.Collections;
using UnityEngine;

public class Miniature : MonoBehaviour
{
    public bool StartMiniature,MinPlay;
    GameObject battery;

    SceneManage SM;

    void Start()
    {
       
        SM = GameObject.Find("MainCamera").GetComponent<SceneManage>();
       

        battery = SM.objRJ.transform.Find("Battery").gameObject;
        if (SM.SL.ShowInfo("Planet0") == "15/0/1/1/1.0/1/2.1.0.0.0.0/0.0.0.0.0.0/0.0.0.0.0.0/0.0.0.0.0.0/0.0.0.0.0.0/0.0.0.0.0.0/0.0.0.0.0.0/0.0.0.0.0.0/0.0.0.0.0.0/0.0.0.0.0.0/0.0.0.0.0.0/0.0.0.0.0.0/0.0.0.0.0.0/0.0.0.0.0.0/0.0.0.0.0.0/0.0.0.0.0.0/0.0.0.0.0.0/0.0.0.0.0.0/0.0.0.0.0.0/0.0.0.0.0.0")
        {
            SM.objRJ.GetComponent<RobotEnginer>().Min = true;
            SM.objBlack.SetActive(true);
            StartMiniature = true;
            battery.SetActive(false);
            SM.Factory.orderPump = 2;
            SM.Factory.CheckOrderPump();
        }
        else
        {
            Destroy(SM.objBat);
            Destroy(SM.objBlack);
        }
    }
    private void FixedUpdate()
    {
        if (StartMiniature)
        {  
            StartMin();
        }
    }
    private void StartMin()
    {

        Camera.main.transform.position = new Vector3(SM.objRJ.transform.position.x, Camera.main.transform.position.y, -10);
        SM.objRJ.GetComponent<RobotEnginer>().Miniature(SM.objBat.transform.position);
        if (SM.objRJ.GetComponent<RobotEnginer>().Give)
        {
            gameObject.GetComponent<AudioSource>().Play();
            transform.Find("Lights").GetComponent<ParticleSystem>().Play();
            SM.objBat.transform.position = new Vector2(SM.objRJ.transform.position.x, SM.objRJ.transform.position.y);

            SM.Score.Change("F","-",999);
            battery.SetActive(true); 
            Destroy(SM.objBlack);
            StartMiniature = false;
            StartCoroutine(Destroy());
        }
    }

    IEnumerator Destroy()
    {
        yield return new WaitForSeconds(1);
        Destroy(gameObject);
        SM.objHint.SetActive(true);
    }

}
