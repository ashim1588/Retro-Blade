using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dragon : MonoBehaviour
{

    Animator anim;
    public float speed;
    int dir;
    float dirTimer = .7f;
    public int health;
    public GameObject deathParticle;
    bool canAttack;
    float attackTimer = 2f;
    public GameObject projectile;
    public float thrustPower;
    float changeTimer = .2f;
    bool shouldChange;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        Random.Range(0, 4);
        canAttack = false;
        shouldChange = false;

    }

    // Update is called once per frame
    void Update()
    {
        dirTimer -= Time.deltaTime;
        if(dirTimer <= 0)
        {
            dirTimer = .7f;
            dir = Random.Range(0, 4);
            }
        Movement();
        attackTimer -= Time.deltaTime; 
       if(attackTimer<= 0)
        {
            attackTimer = 2f;
            canAttack = true;
        }
        Attack();
        if(shouldChange)
        {
            changeTimer -= Time.deltaTime;
            if(changeTimer <= 0)
            {
                changeTimer = .2f;
                shouldChange = false;
            }
        }
    }

    void Attack()
    {
        if (!canAttack)
            return;
        canAttack = false;
       if(dir == 0)
        {
        GameObject newProjectile =  Instantiate(projectile, transform.position, transform.rotation);
            newProjectile.GetComponent<Rigidbody2D>().AddForce(Vector2.up * thrustPower);
        }  
      else if(dir == 1)
        {
            GameObject newProjectile = Instantiate(projectile, transform.position, transform.rotation);
            newProjectile.GetComponent<Rigidbody2D>().AddForce(Vector2.left * thrustPower);
        }
      else if(dir == 2)
        {
            GameObject newProjectile = Instantiate(projectile, transform.position, transform.rotation);
            newProjectile.GetComponent<Rigidbody2D>().AddForce(Vector2.down * thrustPower);
        }
      else if(dir == 3)
        {
            GameObject newProjectile = Instantiate(projectile, transform.position, transform.rotation);
            newProjectile.GetComponent<Rigidbody2D>().AddForce(Vector2.right * thrustPower);
        }
     
    }

    void Movement()
    {
        if (dir == 0)
        {
            transform.Translate(0, speed * Time.deltaTime, 0);
            anim.SetInteger("dir", dir);
        }
        else if (dir == 1)
        {
            transform.Translate(-speed * Time.deltaTime, 0, 0);
            anim.SetInteger("dir", dir);
        }
        else if (dir == 2)
        {
            transform.Translate(0, -speed*Time.deltaTime, 0);
            anim.SetInteger("dir", dir);
        }
        else if (dir == 3)
        {
            transform.Translate(speed * Time.deltaTime, 0, 0);
            anim.SetInteger("dir", dir);
        }

    }

     private void OnTriggerEnter2D(Collider2D collision)
    {
       if(collision.gameObject.tag == "Sword")
        {
            health--;
            collision.gameObject.GetComponent<Sword>().CreateParticle();
            GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().canAttack = true;
            GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().canMove = true;
            Destroy(collision.gameObject);
            if(health<=0)
            {
                Instantiate(deathParticle, transform.position, transform.rotation);
                Destroy(gameObject);
            }
        }
    }

     private void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.tag == "Player")
        {
            health--;
            if (!coll.gameObject.GetComponent<Player>().iniFrames)
            {
                coll.gameObject.GetComponent<Player>().currentHealth--;
                coll.gameObject.GetComponent<Player>().iniFrames = true;
            }
            if (health <= 0)
            {
                Instantiate(deathParticle, transform.position, transform.rotation);
                Destroy(gameObject);
            }
        }
        else if (coll.gameObject.tag == "Wall")
        {
            if (shouldChange)
                return;
            if (dir == 0) dir = 2;
            else if (dir == 1) dir = 3;
            else if (dir == 2) dir = 0;
            else if (dir == 3) dir = 1;
            shouldChange = true;
        }
        GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().canMove = true;
    }
}
