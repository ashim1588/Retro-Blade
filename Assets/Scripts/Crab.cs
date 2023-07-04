using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crab : MonoBehaviour
{

    public int health;
    public GameObject particleEffect;
    SpriteRenderer spriteRenderer;
    int direction;
    float timer = 2f;
    public float speed;
    public Sprite facingUp;
    public Sprite facingDown;
    public Sprite facingRight;
    public Sprite facingLeft;
    float changeTimer = .2f;
    bool shouldChange;
    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        direction = Random.Range(0,4);
        shouldChange = false;
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        if(timer <= 0)
        {
            timer = 1f;
            direction = Random.Range(0, 4);
        }
        Movement();
        if (shouldChange)
            changeTimer -= Time.deltaTime;
        if(changeTimer <= 0)
        {
            shouldChange = false;
            changeTimer = .2f;
        }
    }

 

    void Movement()
    {
        if(direction == 0)
        {
            transform.Translate(0, -speed * Time.deltaTime, 0);
            spriteRenderer.sprite = facingDown;
        }
        else if(direction == 1)
        {
            transform.Translate(-speed * Time.deltaTime, 0, 0);
            spriteRenderer.sprite = facingLeft;
        }
        else if(direction == 2)
        {
            transform.Translate(speed * Time.deltaTime, 0, 0);
            spriteRenderer.sprite = facingRight;
        }
        else if(direction == 3)
        {
            transform.Translate(0, speed * Time.deltaTime, 0);
            spriteRenderer.sprite = facingUp;
        }

    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Sword")
        {
            health--;
            if (health <= 0)
            {
                Instantiate(particleEffect, transform.position, transform.rotation);
                Destroy(gameObject);
            }
            col.GetComponent<Sword>().CreateParticle();
            GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().canAttack = true;
            GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().canMove = true;
            Destroy(col.gameObject);
        }
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.tag == "Player")
        {
            health--;
            if(!coll.gameObject.GetComponent<Player>().iniFrames)
            {
                coll.gameObject.GetComponent<Player>().currentHealth--;
                coll.gameObject.GetComponent<Player>().iniFrames = true; 
            }
            if (health <= 0)
            {
                Instantiate(particleEffect, transform.position, transform.rotation);
                Destroy(gameObject);
            }
        }
        else if(coll.gameObject.tag == "Wall")
        {
            if (shouldChange)
                return;
            if (direction == 0) direction = 3;
            else if (direction == 1) direction = 2;
            else if (direction == 2) direction = 1;
            else if (direction == 3) direction = 0;
            shouldChange = true;
        }
        GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().canMove = true;
    }


}
