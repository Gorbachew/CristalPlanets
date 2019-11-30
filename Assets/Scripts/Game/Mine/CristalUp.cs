using UnityEngine;

public class CristalUp : MonoBehaviour
{
    public float SpeedCristal = 1f;
    public int value;


    SceneManage lo;

    private void Awake()
    {
      //  Debug.Log("Awake CristalUp");

        lo = GameObject.Find("/MainCamera").GetComponent<SceneManage>();
    }
    private void FixedUpdate()
    {
        transform.position = Vector2.MoveTowards(transform.position,
            new Vector2(transform.position.x, 10),
             SpeedCristal * Time.deltaTime);

       
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name == "Stock")
        {
            lo.Score.Change("C","+",value);
            Destroy(gameObject);
        }
    }



}
