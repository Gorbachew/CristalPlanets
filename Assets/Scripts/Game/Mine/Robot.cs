using System.Collections;
using UnityEngine;

public class Robot : MonoBehaviour
{
    private bool bunchClose,fullBag,checkUpBag,checkUpSpeed,checkUpMine,checkUpEnergy, brakeRobot,checkBreak;
    private float timeMining,Scale,Speed = 0.2f;
    private int valueBag, tank;
    private int Level = 1;
    private Animator animator;
    private MineExpansion mineExpansion;
    private Bunch bunch;
    private Fuel fuel;
    private SpriteRenderer[] spritesRenderer;
    private GameObject energy, track, Leg,Leg2,pickaxe;
    private AudioSource audioSource;
    private void Awake()
    {
        animator = gameObject.GetComponent<Animator>();
        mineExpansion = gameObject.GetComponentInParent<MineExpansion>();
        audioSource = gameObject.GetComponentInChildren<AudioSource>();
        bunch = mineExpansion.GetComponentInChildren<Bunch>();
        fuel = GameObject.Find("CanvasScore/Fuel").GetComponent<Fuel>();
        energy = gameObject.transform.Find("Body/Energy").gameObject;
        energy.SetActive(false);
        track = gameObject.transform.Find("Track").gameObject;
        track.SetActive(false);
        Leg = gameObject.transform.Find("Leg").gameObject;
        Leg2 = gameObject.transform.Find("Leg2").gameObject;
        pickaxe = gameObject.transform.Find("Arm/Pickaxe").gameObject;
        spritesRenderer = gameObject.GetComponentsInChildren<SpriteRenderer>();

        
    }
    private void Start()
    {
        //Разная скорость передвижения, чтобы роботы не сливались
        Speed += Random.Range(0.05f, 0.19f);
        Scale = transform.localScale.x;
        ColorRobot();
    }

    private void FixedUpdate()
    {
        FillTank();
        CheckUp();
        if (!brakeRobot)
        {
            checkBreak = false;
            if (fullBag)
            {
                Move(-1, -Scale);
                animator.SetBool("Off", false);
                animator.SetBool("Mine", false);
            }
            else if (bunchClose && !fullBag)
            {
                Mine();
                animator.SetBool("Off", false);
                animator.SetBool("Mine", true);

            }
            else if (!fullBag)
            {
                Move(1, Scale);
                animator.SetBool("Off", false);
                animator.SetBool("Mine", false);
            }
        }
        else if(!checkBreak)
        {
            checkBreak = true;
            //checkBreak чтобы это исполнилось 1 раз
            audioSource.Play();
            animator.SetBool("Off", true);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (collision.name == "Bunch")
        {
            TimeMining();
            bunchClose = true;
        }
        if (collision.name == "Pipe" && fullBag)
        {
            PutCristal(collision.transform);      
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.name == "Bunch")
        {
            bunchClose = false;
            bunch.Bonus();
        }

    }
    //Идти к куче
    private void Move(float direction,float Scale)
    {
        if (checkUpSpeed) Speed = 1;
        transform.localScale = new Vector2(Scale, transform.localScale.y);
        transform.position = Vector2.MoveTowards(transform.position,
            new Vector2(transform.position.x + direction, transform.position.y),
            Speed * Time.deltaTime);
      
    }
    
    //Добыча кристалов у кучи
    private void Mine()
    {

        if (timeMining <= 0)
        {
            fullBag = true;
            //Просчет сколько добыл кристалов
            valueBag = mineExpansion.numberLevel + mineExpansion.mineBlocks + (int)(Level);
            if (checkUpBag) valueBag *= 2;
        }
        else
        {
            timeMining -= Time.deltaTime;
        }
    }
    //Отдача кристала в трубу
    private void PutCristal(Transform obj)
    {
        fullBag = false;
        GameObject cristal = Instantiate(Resources.Load("Prefab/Cristal"), obj) as GameObject;
        cristal.GetComponent<CristalUp>().value = valueBag;
    }
    //Повышение уровня
    public void LevelUp()
    {
        Level += 1;
        ColorRobot();
    }
    
    
    //Просчет времени добычи
    private void TimeMining()
    {
        if (checkUpMine) timeMining = 5.0f;
        else timeMining = 10.0f;
        if (checkUpBag) timeMining *= 2;
    }
    //Подбор топлива из склада
    private void FillTank()
    {
        if(tank == 0 && fuel.Value() > 0)
        {
            brakeRobot = false;
            int timeJob = 5;
            tank = Level;
            fuel.Minus(tank);
            if (checkUpEnergy) timeJob = 10;
            StartCoroutine(EatFuel(timeJob));
        }
        else if(tank == 0 && fuel.Value() <= 0)
        {
            brakeRobot = true;
        }
    }
    //Потребление топлива
    IEnumerator EatFuel(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        tank = 0;
    }
    //Анимация
    public void knockPickaxe()
    {
        bunch.goDust();    
    }
    //Показ уровня
    public int WatchLevel()
    {
        return Level;
    }
    private void ColorRobot()
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
    //Проверка на покупку улучшения для роботов
    private void CheckUp()
    {
        if (mineExpansion.UpBag && !checkUpBag)
        {
            spritesRenderer[4].sprite = Resources.LoadAll("Sprites/Robots")[11] as Sprite;
            checkUpBag = true;
        }
        if (mineExpansion.UpSpeed && !checkUpSpeed)
        {
            Leg.SetActive(false);
            Leg2.SetActive(false);
            track.SetActive(true);
            checkUpSpeed = true;
        }
        if (mineExpansion.UpMine && !checkUpMine)
        {
            spritesRenderer[6].sprite = Resources.LoadAll("Sprites/Robots")[12] as Sprite;
            pickaxe.SetActive(false);
            checkUpMine = true;
        }
        if (mineExpansion.UpEnergy && !checkUpEnergy)
        {
            energy.SetActive(true);
            checkUpEnergy = true;
        }
    }
}
