using UnityEngine;
using UnityEngine.UI;

public class Cheat : MonoBehaviour
{
    private int count;

    private InputField inputField;
    SceneManage SM;

    private void Awake()
    {
        SM = GameObject.Find("MainCamera").GetComponent<SceneManage>();

       
    }
    private void Start()
    {
        inputField = SM.CanvasPlanet.GetComponentInChildren<InputField>();
        inputField.gameObject.SetActive(false);
    }
    private void OnMouseUpAsButton()
    {
        count += 1;
        if (count == 8)
        {
            inputField.gameObject.SetActive(true);
        }
        if (inputField.text == "bablo") SM.Score.Change("C","+",1000000);
        if (inputField.text == "neft") SM.Score.Change("F", "+", 1000000);
    }


}
