
using System;
using UnityEngine;
using UnityEngine.UI;

public class CameraUpDown : MonoBehaviour
{
    private Text text;
    private Animator animator;
    private AudioSource Elevator;
    private bool goElev = true;
    private int floor = 0;

    SceneManage SM;
    private void Awake()
    {
        Debug.Log("Awake CameraUpDown");
        SM = GameObject.Find("MainCamera").GetComponent<SceneManage>();
        text = gameObject.GetComponentInChildren<Text>();
        animator = gameObject.GetComponent<Animator>();
        Elevator = gameObject.GetComponent<AudioSource>();
    }
    private void FixedUpdate()
    {
        Floor();
    }

    private void Floor()
    {
        if (goElev)
        {
            
            text.text = floor.ToString();
            float startY = transform.position.y;
            float endY = SM.Mines.mineExpansions[floor].transform.position.y + 1.7f;
            transform.position = Vector2.MoveTowards(
             new Vector2(transform.position.x, startY),
             new Vector2(transform.position.x, endY),
             5 * Time.deltaTime);

            SM.Elevator.CameraFix();
            if(startY.ToString("#.##") == endY.ToString("#.##"))
            {
                Elevator.Stop();
                goElev = false;
                animator.SetBool("Up", false);
                animator.SetBool("Down", false);
            }
        }   
    }
    public void Up()
    {
        //Вверх
        floor--;
        if (floor < 0) floor = 0;
        goElev = true;
        animator.SetBool("Up", true);
        Elevator.Play();
    }
    public void Down()
    {
        floor++;
        if (floor >= SM.Mines.LastMine().transform.GetSiblingIndex() + 1) floor--;
        CheckLevel();
        goElev = true;
        animator.SetBool("Down", true);
        Elevator.Play();
    }
    private void CheckLevel()
    {
        int level = int.Parse(SM.PlanetInfo.ShowInfo("Levels", "E"));
        if (floor > level)
        {
            floor--;
        }
    }

    private void OnDisable()
    {
        floor = 0;
      //  Up();
        transform.localPosition = new Vector2(-2.495f, -2.24f);       
    }


}
