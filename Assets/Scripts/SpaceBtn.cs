using UnityEngine;
using UnityEngine.UI;

public class SpaceBtn : MonoBehaviour
{
    private GameObject Canvas, message;
    private Text text;
    public GameObject UI;
 
    SceneManage SM;

    private void Start()
    {
        SM = GameObject.Find("MainCamera").GetComponent<SceneManage>();
        UI = GameObject.Find("/MainCamera");
        message = SM.CanvasSpace.transform.Find("Message").gameObject;
        message.SetActive(false);
        if (gameObject.name == "CanvasUI")
        {
            Canvas = GameObject.Find("/CanvasUI");
            Canvas.GetComponent<Canvas>().worldCamera = UI.GetComponent<Camera>();
            text = Canvas.transform.Find("CanvasSpace/GC").GetComponentInChildren<Text>();
            text.text = GameObject.Find("/SL").GetComponent<SAVELOAD>().ShowInfo("GC");
        }
    }

    public void Btn(string btn)
    {
        SM.BtnSource.Play();
        switch (btn)
        {
            case "Planet0":
                UI.GetComponent<SceneManage>().LoadScene("Planet0");
                break;
            case "Rating":
                UI.GetComponent<SceneManage>().LoadScene("Rating");
                break;
            case "Exit":
                UI.GetComponent<SceneManage>().LoadScene("Space");
                break;
            case "Quit":
                Application.Quit();
                break;
        }
        
    }
    public void Hint(bool On)
    {
        SM.BtnSource.Play();
        if (On) SM.objHint.SetActive(true);
        else SM.objHint.SetActive(false);
    }
    public void Message(bool On)
    {
        SM.BtnSource.Play();
        if (On) message.SetActive(true);
        else message.SetActive(false);
    }
    
}
