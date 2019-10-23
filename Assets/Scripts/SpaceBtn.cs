
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SpaceBtn : MonoBehaviour
{
    private Text text;
    private SAVELOAD saveload;
    private AudioSource audioSource;


    private void Awake()
    {
        audioSource = gameObject.GetComponent<AudioSource>();
        if (gameObject.name == "Rating")
        {
            text = GameObject.Find("GC").GetComponentInChildren<Text>();
            saveload = GameObject.Find("SL").GetComponent<SAVELOAD>();
        }
    }
    private void Start()
    {
        DisplayGC();
    }

    private void DisplayGC()
    {
        if (gameObject.name == "Rating")
        {
            text.text = saveload.ShowInfo("GC");
                
        }
    }
    private void OnMouseUpAsButton()
    {
        audioSource.Play();

        switch (gameObject.name)
        {
            case "Rating":
                SceneManager.LoadScene("Rating");
                break;
            case "Quit":
                Application.Quit();
                break;
        }
    }
    
    
}
