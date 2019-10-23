using System.Collections;
using UnityEngine;

public class Bunch : MonoBehaviour
{
    ParticleSystem particleSystems;
    AudioSource audioSource, audioBonus;
    SpriteRenderer iconBonus;
    Table table;
    private bool checkBonus;
    // Start is called before the first frame update
    void Start()
    {
        iconBonus = gameObject.transform.Find("Bonus/Icon").GetComponent<SpriteRenderer>();
        particleSystems = gameObject.GetComponentInChildren<ParticleSystem>();
        audioSource = gameObject.transform.Find("Bunch").GetComponent<AudioSource>();
        audioBonus = gameObject.transform.Find("Bonus").GetComponent<AudioSource>();

        table = Camera.main.GetComponent<Table>();
        iconBonus.gameObject.SetActive(false);
    }

    public void goDust()
    {
        
        particleSystems.Play();
        audioSource.Play();
    }



    public void Bonus()
    {
        if (!checkBonus)
        {
            
            int random = Random.Range(1, 100);
         
            if (random >= 90)
            {
                checkBonus = true;
                audioBonus.Play();
                iconBonus.gameObject.SetActive(true);
                StartCoroutine(DestroyBonus());
                if (random > 99)
                {
                    iconBonus.sprite = Resources.LoadAll("Sprites/Textures2")[23] as Sprite;
                }
                else if (random < 96)
                {
                    iconBonus.sprite = Resources.LoadAll("Sprites/Textures2")[24] as Sprite;
                }
                else if (random < 93)
                {
                    iconBonus.sprite = Resources.LoadAll("Sprites/Textures2")[25] as Sprite;
                }
            }
            
        }

        
        
    }
    public void ClickBonus()
    {
        checkBonus = false;
        iconBonus.gameObject.SetActive(false);
        table.BonusFactory();
    }

    IEnumerator DestroyBonus()
    {
        yield return new WaitForSeconds(30);
        iconBonus.gameObject.SetActive(false);
        checkBonus = false;
    }

}
