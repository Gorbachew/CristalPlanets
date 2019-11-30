using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneManage : MonoBehaviour
{
    public SAVELOAD SL;
    public GameObject rCam, pCam, sCam;

    public GameObject Space, Rating, Planet, UI;
    public string letsGo;
    private Vector3 transformCam;
    public CameraMove cameraMove;
    private bool camInPlace;
    //Ссылки на Планетные переменные 
    public GameObject CanvasRating, CanvasSpace, CanvasPlanet;
    public ADMOBManager admob;
    public PlanetInfo PlanetInfo;
    public DisplayPlayers displayPlayers;
    public Elevator Elevator;
    public Score Score;
    public Factory Factory;
    public Mines Mines;
    public Rocket Rocket;
    public Table table;
    public Miniature Miniature;
    public GameObject objEl, objFac, objLab, objStock,objBoer, objRock, objRJ, objBat, objBlack, objTable, objBtnStart,objBonusF, objBonusR, objBonusC,objRegTableCont, objBonusTableCont, objRatingTableCont,objHint;
    public GameObject btnL, btnE, btnF, btnR;
    public Builds[] Builds;
    public AudioSource Sound, BuySourse,BonusSourse, DynSource,ElectroSource,MineSource,CrashSource,BtnSource,ErrorSource;
    public Button iconBtnL, iconBtnE, iconBtnF, iconBtnR;
    public Transform ContentRating;
    public AddUser addUser;
    public Statistic statistic;
    public CameraUpDown cameraUpDown;
    private void Awake()
    {
        SceneManager.LoadSceneAsync("Space", LoadSceneMode.Additive);
        SceneManager.LoadSceneAsync("Rating", LoadSceneMode.Additive);
        SceneManager.LoadSceneAsync("Planet", LoadSceneMode.Additive);
    }
    private void Start()
    {
        SL = GameObject.Find("SL").GetComponent<SAVELOAD>();
        cameraMove = GetComponent<CameraMove>();
        admob = GetComponent<ADMOBManager>();

        rCam = GameObject.Find("RatingCamera");
        sCam = GameObject.Find("SpaceCamera");
        pCam = GameObject.Find("PlanetCamera");

        Planet = GameObject.Find("PlanetLevel");
        Space = GameObject.Find("CanvasSpaces");
        Rating = GameObject.Find("CanvasRatings");
        UI = GameObject.Find("CanvasUI");

        CanvasSpace = UI.transform.Find("CanvasSpace").gameObject;
        CanvasRating = UI.transform.Find("CanvasRating").gameObject;
        CanvasPlanet = UI.transform.Find("CanvasPlanet").gameObject;

        objTable = GameObject.Find("Table");
        objBlack = CanvasPlanet.transform.Find("Black").gameObject;
        objHint = CanvasPlanet.transform.Find("Hint").gameObject;


        Score = CanvasPlanet.transform.Find("Overlay/Score").GetComponent<Score>();
        statistic = CanvasPlanet.transform.Find("Overlay/Statistic").GetComponent<Statistic>();
        objBonusF = CanvasPlanet.transform.Find("Overlay/BonusF").gameObject;
        objBonusR = CanvasPlanet.transform.Find("Overlay/BonusR").gameObject;
        objBonusC = CanvasPlanet.transform.Find("Overlay/BonusC").gameObject;


        PlanetInfo = Planet.GetComponent<PlanetInfo>();

        
        Mines = Planet.transform.Find("Mines").GetComponent<Mines>();
        objEl = Planet.transform.Find("Up/Elevator").gameObject;
        objFac = Planet.transform.Find("Up/Factory").gameObject;
        objLab = Planet.transform.Find("Up/Lab").gameObject;
        objStock = Planet.transform.Find("Up/Stock").gameObject;
        objBoer = Planet.transform.Find("Up/Boer").gameObject;
        objRock = Planet.transform.Find("Up/RocketPlace").gameObject;
        objRJ = Planet.transform.Find("RobotEnginer").gameObject;
        Miniature = Planet.transform.Find("Miniature").GetComponent<Miniature>();
        objBat = Planet.transform.Find("Miniature/BatteryF").gameObject;
        Builds = Planet.transform.Find("Up").GetComponentsInChildren<Builds>();

        cameraUpDown = objEl.GetComponent<CameraUpDown>();
       
        BuySourse = Planet.transform.Find("Up").GetComponent<AudioSource>();
        BonusSourse = Planet.transform.Find("Mines").GetComponent<AudioSource>();
        MineSource = Planet.GetComponent<AudioSource>();
        CrashSource = Planet.transform.Find("Up/Forest").GetComponent<AudioSource>();
        BtnSource = GameObject.Find("CanvasUI").GetComponent<AudioSource>();
        DynSource = Planet.transform.Find("bgPlanet0").GetComponent<AudioSource>();
        ErrorSource = GameObject.Find("SL").GetComponent<AudioSource>();
      

        Elevator = objStock.GetComponent<Elevator>();
        Factory = objFac.GetComponent<Factory>();
        Rocket = objRock.GetComponent<Rocket>();

        btnL = objLab.transform.Find("Btn").gameObject;
        iconBtnL = btnL.transform.GetComponentInChildren<Button>();
        btnE = objStock.transform.Find("Btn").gameObject;
        iconBtnE = btnE.transform.GetComponentInChildren<Button>();
        btnF = objFac.transform.Find("Btn").gameObject;
        iconBtnF = btnF.transform.GetComponentInChildren<Button>();
        btnR = objRock.transform.Find("Btn").gameObject;
        iconBtnR = btnR.transform.GetComponentInChildren<Button>();
        table = GetComponent<Table>();
        objRegTableCont = objTable.transform.Find("RegisterContent").gameObject;
        objBonusTableCont = objTable.transform.Find("BonusContent").gameObject;
        objRatingTableCont = objTable.transform.Find("RatingContent").gameObject;

        displayPlayers = Rating.GetComponent<DisplayPlayers>();
        ContentRating = Rating.transform.Find("Scroll View/Viewport/Content");

        addUser = Rating.GetComponent<AddUser>();

        cameraMove.enabled = false;

      //  table.Load();
       // cameraUpDown.Load();
       // PlanetInfo.Load();
       // SL.Load();
       //Score.Load();

        rCam.SetActive(false);
        sCam.SetActive(false);
        pCam.SetActive(false);
        
        Planet.SetActive(false);
        Rating.SetActive(false);

        CanvasRating.SetActive(false);
        CanvasPlanet.SetActive(false);
        
        objTable.SetActive(false);
        objBlack.SetActive(false);
        objHint.SetActive(false);

        objBonusF.SetActive(false);
        objBonusR.SetActive(false);
        objBonusC.SetActive(false);

      

        

    }
    private void FixedUpdate()
    {
        LetsGoCam();

    }
    private void LetsGoCam()
    {
        if(transform.position != transformCam && !camInPlace)
        {
            transform.position = Vector3.MoveTowards(transform.position, transformCam, 200 * Time.deltaTime);
            if (transform.position == transformCam) camInPlace = true;
        }
    }

    public void LoadScene(string Scene)
    {
        letsGo = Scene;
        camInPlace = false;

        admob.InterstialShow();
        switch (Scene)
        {
            case "Rating":
                cameraMove.enabled = false;
                

                Rating.SetActive(true);
                Space.SetActive(false);
 
                CanvasRating.SetActive(true);
                CanvasSpace.SetActive(false);

                transformCam = rCam.transform.position;

                addUser.Load();
                displayPlayers.LoadPlayers();
                break;
            case "Space":

                cameraMove.enabled = false;
                //Генерирует строку для сохранения планеты
                if (Planet.activeSelf) SL.CollectionData("Planet0",PlanetInfo.GenerateStrPlanets());
                objTable.SetActive(false);
                Rating.SetActive(false);
                Space.SetActive(true);
                Planet.SetActive(false);

                CanvasPlanet.SetActive(false);
                CanvasRating.SetActive(false);
                CanvasSpace.SetActive(true);

                transformCam = sCam.transform.position;
                SL.LoadInfo();
            break;
            case "Planet0":
                
                cameraMove.enabled = true;

                Planet.SetActive(true);
                Space.SetActive(false);
                CanvasPlanet.SetActive(true);
                CanvasSpace.SetActive(false);

                transformCam = pCam.transform.position;

                PlanetInfo.LoadInfo();
                break;
        }
    }
}   