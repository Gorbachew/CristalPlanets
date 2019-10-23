
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    private bool block, blockElevator;
    private GameObject elevator;
    private float speedElevator;
    private PlanetInfo planetInfo;
    private Animator animator;
    private AudioSource audioSourse;
    private void Awake()
    {
        
       
        planetInfo = Camera.main.GetComponent<PlanetInfo>();
        if(gameObject.name == "Up" || gameObject.name == "Down")
        {
            elevator = gameObject.transform.parent.gameObject;
            animator = elevator.GetComponent<Animator>();
            
            audioSourse = elevator.GetComponent<AudioSource>();
        }
    }

    private void OnMouseDrag()
    {
        if (!block)
        {
            switch (gameObject.name)
            {
                case "Right":
                    Camera.main.transform.position = Vector3.MoveTowards(Camera.main.transform.position,
                    new Vector3(Camera.main.transform.position.x + 3f, Camera.main.transform.position.y, Camera.main.transform.position.z),
                    5.0f * Time.deltaTime);
                    break;
                case "Left":
                    Camera.main.transform.position = Vector3.MoveTowards(Camera.main.transform.position,
                    new Vector3(Camera.main.transform.position.x - 3f, Camera.main.transform.position.y, Camera.main.transform.position.z),
                    5.0f * Time.deltaTime);
                    break;
                
            }
        }
        if (!blockElevator)
        {
            
            switch (gameObject.name)
            {
                case "Up":
        
                    elevator.transform.position = Vector2.MoveTowards(elevator.transform.position, new Vector2(elevator.transform.position.x,
                                    elevator.transform.position.y + 1f), speedElevator * Time.deltaTime);
                    break;
                case "Down":
         
                    elevator.transform.position = Vector2.MoveTowards(elevator.transform.position, new Vector2(elevator.transform.position.x,
                                    elevator.transform.position.y - 1f), speedElevator * Time.deltaTime);
                    break;
            }
        }   
    }
    private void OnMouseDown()
    {
        
        if (gameObject.name == "Up" || gameObject.name == "Down")
        {
            audioSourse.Play();
            animator.speed = int.Parse(planetInfo.ShowInfo("Levels", "L")) * 0.5f;
            switch (gameObject.name)
            {
                case "Up":
                    animator.SetBool("Up", true);
                    break;
                case "Down":
                    animator.SetBool("Down", true);
                    break;
            }
        }
    }
    private void OnMouseUp()
    {
        if (gameObject.name == "Up" || gameObject.name == "Down")
        {
            audioSourse.Stop();
            animator.speed = 1;
            switch (gameObject.name)
            {
                case "Up":
                    animator.SetBool("Up", false);
                    break;
                case "Down":
                    animator.SetBool("Down", false);
                    break;
            }
        }
    }
    
    private void OnCollisionEnter2D(Collision2D collision)
    {   if(gameObject.name == "Right" || gameObject.name == "Left")
        {
            if (collision.gameObject.name == "Block")
            {
                block = true;
            }
        }
        if(gameObject.name == "Up" || gameObject.name == "Down")
        {
            if (collision.gameObject.name == "BlockElevator" || collision.gameObject.name == "Boer")
            {
                blockElevator = true;

            }
        }
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (gameObject.name == "Up" || gameObject.name == "Down")
        {
            if (collision.gameObject.name == "Lvl8" && int.Parse(planetInfo.ShowInfo("Levels", "E")) < 80) blockElevator = true;
            else if (collision.gameObject.name == "Lvl7" && int.Parse(planetInfo.ShowInfo("Levels", "E")) < 70) blockElevator = true;
            else if (collision.gameObject.name == "Lvl6" && int.Parse(planetInfo.ShowInfo("Levels", "E")) < 60) blockElevator = true;
            else if (collision.gameObject.name == "Lvl5" && int.Parse(planetInfo.ShowInfo("Levels", "E")) < 50) blockElevator = true;
            else if (collision.gameObject.name == "Lvl4" && int.Parse(planetInfo.ShowInfo("Levels", "E")) < 40) blockElevator = true;
            else if (collision.gameObject.name == "Lvl3" && int.Parse(planetInfo.ShowInfo("Levels", "E")) < 30) blockElevator = true;
            else if (collision.gameObject.name == "Lvl2" && int.Parse(planetInfo.ShowInfo("Levels", "E")) < 20) blockElevator = true;
            else if (collision.gameObject.name == "Lvl1" && int.Parse(planetInfo.ShowInfo("Levels", "E")) < 10) blockElevator = true; 
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (gameObject.name == "Up" || gameObject.name == "Down")
        {
            if (collision.gameObject.name == "Lvl1" )
            {
                blockElevator = false;
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Block")
        {
            block = false;
        } 
        if (collision.gameObject.name == "BlockElevator" || collision.gameObject.name == "Boer")
        {
            blockElevator = false;
        }   
    }

    public void PlusSpeed(float Speed)
    {
        speedElevator = Speed;
    }

}
