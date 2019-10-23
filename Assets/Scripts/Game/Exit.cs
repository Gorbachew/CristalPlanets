using UnityEngine;
using UnityEngine.SceneManagement;

public class Exit : MonoBehaviour
{

    private void Awake()
    {
        
    }
    private void OnMouseUpAsButton()
    {
        gameObject.GetComponent<AudioSource>().Play();
        SceneManager.LoadScene("Space");
    }
}
