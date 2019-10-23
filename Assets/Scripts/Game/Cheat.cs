using UnityEngine;
using UnityEngine.UI;

public class Cheat : MonoBehaviour
{
    private int count;
    private Score score;
    private Fuel fuel;
    private InputField inputField;
    private void Awake()
    {
        score = GameObject.Find("CanvasScore/Score").GetComponent<Score>();
        fuel = GameObject.Find("CanvasScore/Fuel").GetComponent<Fuel>();
        inputField = gameObject.GetComponentInChildren<InputField>();
    }
    private void Start()
    {
        inputField.gameObject.SetActive(false);
    }
    private void OnMouseUpAsButton()
    {
        count += 1;
        if (count == 8)
        {
            inputField.gameObject.SetActive(true);
        }
        if (inputField.text == "bablo") score.Plus(1000000);
        if (inputField.text == "neft") fuel.Plus(1000000);
    }


}
