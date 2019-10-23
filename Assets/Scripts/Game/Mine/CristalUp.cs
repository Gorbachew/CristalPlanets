using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CristalUp : MonoBehaviour
{
    public float SpeedCristal = 1f;
    public int value;
    private Score score;
    private void Start()
    {
        score = GameObject.Find("Score").GetComponent<Score>();
    }
    private void FixedUpdate()
    {
        transform.position = Vector2.MoveTowards(transform.position,
            new Vector2(transform.position.x, 10),
             SpeedCristal * Time.deltaTime);

        CristalScale();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name == "Stock")
        {
            score.Plus(value);
            Destroy(gameObject);
        }
    }

    private void CristalScale()
    {
        
        if (value > 5000)
        {
            gameObject.transform.localScale = new Vector2(0.9f, 0.9f);
        }
        else if (value > 1000)
        {
            gameObject.transform.localScale = new Vector2(0.8f, 0.8f);
        }
        else if (value > 500)
        {
            gameObject.transform.localScale = new Vector2(0.7f, 0.7f);
        }
        else if (value > 50)
        {
            gameObject.transform.localScale = new Vector2(0.6f, 0.6f);
        }
        else if (value > 10)
        {
            gameObject.transform.localScale = new Vector2(0.5f, 0.5f);
        }
        else if(value > 0)
        {
            gameObject.transform.localScale = new Vector2(0.4f, 0.4f);
        }
    }

}
