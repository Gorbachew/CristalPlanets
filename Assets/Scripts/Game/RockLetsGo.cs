using UnityEngine;

public class RockLetsGo : MonoBehaviour
{
    Rocket rocket;

    private void Awake()
    {
        rocket = GameObject.Find("Up/RocketPlace").GetComponent<Rocket>();
    }
    private void OnMouseUpAsButton()
    {
        rocket.StartRocketVoid();     
    }

}
