using UnityEngine;

public class RobotEnginer : MonoBehaviour
{

    private float Speed = 0.3f,timeMove,timeIdle = 10f, Scale;
    private Animator animator;
    private int moveDirection;
    public bool Min,Give;
    private void Awake()
    {
        animator = gameObject.GetComponent<Animator>();
    }
    private void Start()
    {
        Scale = transform.localScale.x;
    }
    private void FixedUpdate()
    {

        if (!Min)
        {
            if (timeMove >= 0)
            {
                timeIdle = 10;
                timeMove -= Time.deltaTime;
                animator.SetBool("Move", true);
                switch (moveDirection)
                {
                    case 1:
                        Move(-1, -Scale);
                        break;
                    case 2:
                        Move(1, Scale);
                        break;
                } 
            }
            else
            {
                animator.SetBool("Move", false);
                timeIdle -= Time.deltaTime;
                if (timeIdle <= 0)
                {
                    timeMove = Random.Range(1, 10);
                    moveDirection = Random.Range(1, 3);

                }
            }
        }

        

    }

    public void Move(float direction, float Scale)
    {
        transform.localScale = new Vector2(Scale, transform.localScale.y);
        transform.position = Vector2.MoveTowards(transform.position,
            new Vector2(transform.position.x + direction, transform.position.y),
            Speed * Time.deltaTime);
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.name == "Block")
        {
            if (moveDirection == 2) moveDirection = 1;
            else if (moveDirection == 1) moveDirection = 2;
        }
        

    }
        
    public void Anim(string Anim,bool Bool)
    {
        animator.SetBool(Anim, Bool);
    }


    public void Miniature(Vector2 Battrey)
    {

        if (Min)
        {
            if (transform.position.x >= Battrey.x + 0.4f)
            {
                transform.localScale = new Vector2(-Scale, transform.localScale.y);
                transform.position = Vector2.MoveTowards(transform.position,
                    new Vector2(Battrey.x + 0.3f, transform.position.y),
                    Speed * Time.deltaTime);
                animator.SetBool("Move", true);
            }
            else
            {
                

                animator.SetBool("Move", false);
                animator.SetBool("Give", true);
                
            }
        }
      
    }

    public void MiniatureGiveOff()
    {
        Give = true;
        animator.SetBool("Give", false);
        Min = false;
    }
}
