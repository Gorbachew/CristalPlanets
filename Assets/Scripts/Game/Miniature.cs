using System.Collections;
using UnityEngine;

public class Miniature : MonoBehaviour
{

    SAVELOAD SL;
    Factory factory;
    GameObject Eng,Black, Battery,Robot,BatteryFactory;
    Fuel fuel;
    RobotEnginer robotEnginer;
    ParticleSystem particleSystem;
    public bool StartMiniature,MinPlay;
    private bool Give;

    private void Awake()
    {

        SL = GameObject.Find("SL").GetComponent<SAVELOAD>();
        Eng = GameObject.Find("RobotEnginer");
        Black = GameObject.Find("CanvasScore/Black");
        fuel = GameObject.Find("CanvasScore/Fuel").GetComponent<Fuel>();
        Robot = GameObject.Find("RobotEnginer");
        Battery = Robot.transform.Find("Battery").gameObject;
        BatteryFactory = gameObject.transform.Find("BatteryF").gameObject;
        particleSystem = gameObject.transform.Find("Lights").GetComponent<ParticleSystem>();
        factory = GameObject.Find("Up/Factory").GetComponent<Factory>();
        robotEnginer = Robot.GetComponent<RobotEnginer>();
        robotEnginer.Min = true;
        Black.SetActive(false);
    }
    // Start is called before the first frame update
    void Start()
    {
        if(SL.ShowInfo("Planet1") == "15/0/1/1/1/1/1.1.0.0.0.0")
        {
            Black.SetActive(true);
            StartMiniature = true;
            Battery.SetActive(false);
            factory.OrderPump(true);
            factory.OrderPump(true);
        }
        else
        {
            Destroy(BatteryFactory);
            Destroy(Black);
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

        Camera.main.transform.position = new Vector3(Eng.transform.position.x, Camera.main.transform.position.y, -10);
        robotEnginer.Miniature(BatteryFactory.transform.position);
        if (robotEnginer.Give)
        {
            gameObject.GetComponent<AudioSource>().Play();
            particleSystem.Play();
            BatteryFactory.transform.position = new Vector2(Eng.transform.position.x, Eng.transform.position.y);
                
              
            //BatteryFactory.transform.position = Vector2.MoveTowards(BatteryFactory.transform.position,
            //    new Vector2(Eng.transform.position.x, Eng.transform.position.y),
            //    1f * Time.deltaTime);
            fuel.Minus(999);
            Battery.SetActive(true); 
            Destroy(Black);
            StartMiniature = false;
            StartCoroutine(Destroy());
        }
    }

    IEnumerator Destroy()
    {
        yield return new WaitForSeconds(1);
        Destroy(gameObject);
    }

}
