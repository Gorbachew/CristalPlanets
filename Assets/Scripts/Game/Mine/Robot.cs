using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class Robot : MonoBehaviour
{
    [SerializeField]

    private bool bunchClose, fullBag, brakeRobot;
    private float Scale,Speed,UpSpeed,BonusSpeed, UpBonusSpeed, mineTime = 0, robotSpeed;
    [SerializeField]
    private int tank;
    public int valueBag,fuelNeed;
    private int Level, indexMine;
    private Animator animator;
    private MineExpansion mineExpansion;
    private Bunch bunch;
    private SpriteRenderer[] spritesRenderer;
    private GameObject energy, track, Leg,Leg2,pickaxe;
    [SerializeField]
    private string State;
    SceneManage SM;


    private void Awake()
    {
        Debug.Log("Awake Robot");
        SM = GameObject.Find("/MainCamera").GetComponent<SceneManage>();
        animator = gameObject.GetComponent<Animator>();
        mineExpansion = gameObject.GetComponentInParent<MineExpansion>();
        bunch = mineExpansion.GetComponentInChildren<Bunch>();
        energy = gameObject.transform.Find("Body/Energy").gameObject;
        energy.SetActive(false);
        track = gameObject.transform.Find("Track").gameObject;
        track.SetActive(false);
        Leg = gameObject.transform.Find("Leg/Leg2").gameObject;
        Leg2 = gameObject.transform.Find("Leg2/Leg2").gameObject;
        pickaxe = gameObject.transform.Find("Arm/Pickaxe").gameObject;
        spritesRenderer = gameObject.GetComponentsInChildren<SpriteRenderer>();

        //Разная скорость передвижения, чтобы роботы не сливались
        Scale = transform.localScale.x;

        Speed = Random.Range(0.25f, 0.5f);
        UpSpeed = Random.Range(1f, 1.5f);
        BonusSpeed = Random.Range(1.5f, 2f);
        UpBonusSpeed = Random.Range(3f, 4f);
    }
    public void Load()
    {

        if (gameObject.activeSelf)
        {   
            indexMine = mineExpansion.indexMine;
            ValueBag();
            RobotSpeed();
            FillTank();
            fuelNeed = Level + mineExpansion.indexMine;
            
          
        }
        
        
      
    }

    private void FixedUpdate()
    {
        //То, что зависит от Time.deltaTime
        switch (State)
        {
            case "Move":
                Move(1, Scale);
                animator.SetBool("Move", true);
                animator.SetBool("Mine", false);
                break;
            case "MoveBack":
                Move(-1, -Scale);
                animator.SetBool("Move", true);
                animator.SetBool("Mine", false);
                break;
            case "Mine":
                Mining();
                animator.SetBool("Move", false);
                animator.SetBool("Mine", true);
                break;
            case "NoFuel":
                WaitFuel();
                animator.SetBool("Move", false);
                animator.SetBool("Mine", false);
                break;
        }
    }

    //Все, что должно произойти 1 раз за действие. animator очень странно себя ведет, он постоянно обновляется
    private void Logics()
    { 
        if (!brakeRobot)
        {
            //brakeRobotBool = false;
            //Если есть топливо
            if (fullBag)
            {
                //Если полный рюкзак
                State = "MoveBack";
                ValueBag();
            }
            else
            {
                
                if (bunchClose)
                {
                    //Если близко шахта
                    MiningTime();
                    State = "Mine";                  
                }
                else
                {
                    //Если пустой рюкзак
                    State = "Move";                  
                }
                
            }
        }
        //&& !brakeRobotBool
        else if (brakeRobot )
        {
            // //Чтобы не вызывалось многократно 
            // brakeRobotBool = true;
            //Если нет топлива
            State = "NoFuel";
            SM.CrashSource.Play();
            
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name == "Bunch")
        {
            bunchClose = true;
            bunch.Bonus(); 
            Logics();
            
        }
        if (collision.name == "Pipe" && fullBag)
        {
            //Отдача кристала в трубу
            fullBag = false;
            GameObject cristal = Instantiate(Resources.Load("Prefab/Cristal"), collision.transform) as GameObject;
            cristal.GetComponent<CristalUp>().value = valueBag;
            FillTank();
            tank -= 1;
        }
       
    }

  
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.name == "Bunch")
        {
            bunchClose = false;
            Logics();

        }
      //  Logics();
    }

    //Идти к куче
    private void Move(float direction,float Scale)
    {
        

        //Передвигает модель
        transform.localScale = new Vector2(Scale, transform.localScale.y);
        transform.position = Vector2.MoveTowards(transform.position,
            new Vector2(transform.position.x + direction, transform.position.y),
            robotSpeed * Time.deltaTime);
      
    }
   
    //Подбор топлива из склада
    private void FillTank()
    {
        if (gameObject.activeSelf)
        {
            
            fuelNeed = Level + mineExpansion.indexMine;
            if (tank <= 0 && SM.Score.Value("F") >= fuelNeed)
            {
                brakeRobot = false;
                //Если не работает бонус, то вычитаем топливо
                if (!SM.objBonusR.activeSelf) SM.Score.Change("F", "-", fuelNeed);
                //Сколько циклов робот может принести добычу
                if (SM.Mines.MINESINFO[indexMine][4] == 1) tank = 2;
                else tank = 1;
            }
            else if (tank <= 0 && SM.Score.Value("F") < fuelNeed) brakeRobot = true;

            RobotSpeed();


            Logics();
        }
       
    }



    private void RobotSpeed()
    {
        //Выдает скорость роботам

        if (SM.objBonusR.activeSelf)
        {
            animator.speed = 2f;
            robotSpeed = BonusSpeed;

            if (SM.Mines.MINESINFO[indexMine][3] == 1) robotSpeed = UpBonusSpeed;
        }
        else if (SM.Mines.MINESINFO[indexMine][3] == 1)
        {
            animator.speed = 1.5f;
            robotSpeed = UpSpeed;
        }
        else
        {
            animator.speed = 1f;
            robotSpeed = Speed;
        }
    }


    //Всякие маленькие методы
    //Ждет когда топливо когда его нет
    private void WaitFuel()
    {
        if (SM.Score.Value("F") > 0) FillTank();
    }
    
    private void ValueBag()
    {
        //Просчет сколько добыл кристалов
        valueBag = (indexMine + 1) * ((SM.Mines.MINESINFO[indexMine][0] + 1) * Level);
        if (SM.Mines.MINESINFO[indexMine][2] == 1) valueBag *= 2;
        if (SM.objBonusC.activeSelf) valueBag *= 5;
    }
    //Добыча кристалов у кучи
    private void Mining()
    {
        mineTime -= Time.deltaTime;
        if (mineTime <= 0)
        {
            fullBag = true;
            bunchClose = false;
            Logics();
        }
    }
    //Выдает в зависимости от улучшений время добычи
    private void MiningTime()
    {
        mineTime = 6;
        if (SM.objBonusR.activeSelf) mineTime = 2;
        else if (SM.Mines.MINESINFO[indexMine][3] == 1) mineTime = 3;
    }
    //Повышение уровня
    public void LevelUp(int level)
    {
        Level = level;
        ColorRobot();
    }
    //Анимация
    public void knockPickaxe()
    {
        bunch.goDust();
    }

    private void ColorRobot()
    {
        if (spritesRenderer != null)
        {
            foreach (SpriteRenderer robotPart in spritesRenderer)
            {
                if (robotPart.gameObject.name == "Helmet" || robotPart.gameObject.name == "Arm1" || robotPart.gameObject.name == "Bag" || robotPart.gameObject.name == "Leg1")
                {
                    switch (Level)
                    {
                        case 1:
                            robotPart.color = Color.yellow;
                            break;
                        case 2:
                            robotPart.color = Color.red;
                            break;
                        case 3:
                            robotPart.color = Color.green;
                            break;
                        case 4:
                            robotPart.color = Color.blue;
                            break;
                        case 5:
                            //Феолетовый
                            robotPart.color = new Color(0.4f, 0, 0.9f);
                            break;
                        case 6:
                            //Пастельно красный
                            robotPart.color = new Color(0.8f, 0.3f, 0.5f);
                            break;
                        case 7:
                            //Горчичный
                            robotPart.color = new Color(0.4f, 0.2f, 0);
                            break;
                        case 8:
                            //Берюзовый
                            robotPart.color = new Color(0.4f, 0.9f, 174);
                            break;
                        case 9:
                            //Розовый
                            robotPart.color = new Color(0.8f, 0.4f, 1);
                            break;
                        case 10:
                            //Черный
                            robotPart.color = new Color(0, 0, 0);
                            break;
                    }
                }

            }
        }
    }
    //Проверка на покупку улучшения для роботов
    public void CheckUp(int indexMine)
    {
        if (gameObject.activeSelf)
        {
            if (SM.Mines.MINESINFO[indexMine][2] == 1) spritesRenderer[4].sprite = Resources.LoadAll("Sprites/Robots")[11] as Sprite;
            else spritesRenderer[4].sprite = Resources.LoadAll("Sprites/Robots")[3] as Sprite;
            if (SM.Mines.MINESINFO[indexMine][3] == 1)
            {
                Leg.SetActive(false);
                Leg2.SetActive(false);
                track.SetActive(true);
            }
            else
            {
                Leg.SetActive(true);
                Leg2.SetActive(true);
                track.SetActive(false);
            }
            if (SM.Mines.MINESINFO[indexMine][4] == 1)
            {
                spritesRenderer[6].sprite = Resources.LoadAll("Sprites/Robots")[12] as Sprite;
                pickaxe.SetActive(false);
            }
            else
            {
                spritesRenderer[6].sprite = Resources.LoadAll("Sprites/Robots")[7] as Sprite;
                pickaxe.SetActive(true);
            }
            if (SM.Mines.MINESINFO[indexMine][5] == 1) energy.SetActive(true);
            else energy.SetActive(false);

        }
        
    }
    private void OnDisable()
    {
        bunchClose = false; fullBag = false; brakeRobot = false;
        State = null;
        valueBag = 0; fuelNeed = 0;
        transform.localPosition = new Vector2(0, 0);
        if (animator.GetBool("Move")) animator.SetBool("Move", false);
        if (animator.GetBool("Mine")) animator.SetBool("Mine", false);
    }
}
