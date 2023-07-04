using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public float speed;
    Animator anim;
    public Image[] hearts;
    public int maxHealth;
    public int currentHealth;
    public GameObject sword;
    public float thrustPower;
    public bool canMove;
    public bool canAttack;
    public bool iniFrames;
    SpriteRenderer sr;
    float iniTimer = 1f;
  //  public Camera cam;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        if(PlayerPrefs.HasKey("max"))
         { 
            LoadGame();
        } else
          currentHealth = maxHealth;
      
        getHealth();
        canMove = true;
        canAttack = true;
        iniFrames = false;
        sr = GetComponent<SpriteRenderer>();
    }

    void getHealth()
    {
        for (int i = 0; i <= hearts.Length - 1; i++)
        {
            hearts[i].gameObject.SetActive(false);
        }
        for (int i = 0; i <= currentHealth - 1; i++)
        {
            hearts[i].gameObject.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
      //  cam.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z -5);
      //  getHealth();
        Movement();
        if (Input.GetKeyDown(KeyCode.Space))
            Attack();

        if (currentHealth > maxHealth)
            currentHealth = maxHealth; 
        if(iniFrames == true)
        {
            iniTimer -= Time.deltaTime;
            int rn = Random.Range(0, 100);
            if (rn < 50) sr.enabled = false;
            if (rn > 50) sr.enabled = true;
            if (iniTimer <= 0)
            {
                iniTimer = 1f;
                iniFrames = false;
                sr.enabled = true;
            }
        }
        getHealth();
    }

    void Attack()
    {

        if (!canAttack)
            return;
        canMove = false;
        canAttack = false;
        thrustPower = 250;
        GameObject newSword = Instantiate(sword, transform.position, sword.transform.rotation);
        if (currentHealth == maxHealth)
        {
            newSword.GetComponent<Sword>().special = true;
            canMove = true;
            thrustPower = 500;
        }
                #region //Sword Rotation
        int swordDir = anim.GetInteger("dir");
        anim.SetInteger("attackDir", swordDir);
        if (swordDir == 0)
        {
            newSword.transform.Rotate(0, 0, 0);
            newSword.GetComponent<Rigidbody2D>().AddForce(Vector2.up * thrustPower);
        }
        else if (swordDir == 1)
        {
            newSword.transform.Rotate(0, 0, 180);
            newSword.GetComponent<Rigidbody2D>().AddForce(Vector2.up * -thrustPower);
        }
        else if (swordDir == 2)
        {
            newSword.transform.Rotate(0, 0, 90);
            newSword.GetComponent<Rigidbody2D>().AddForce(Vector2.right * -thrustPower);
        }
        else if (swordDir == 3)
        {
            newSword.transform.Rotate(0, 0, -90);
            newSword.GetComponent<Rigidbody2D>().AddForce(Vector2.right * thrustPower);
        }
            #endregion
        

    }

    void Movement()
    {
        if (!canMove)
            return;
        if (Input.GetKey(KeyCode.W))
        {
            transform.Translate(0, speed * Time.deltaTime, 0);
            anim.SetInteger("dir", 0);
            anim.speed = 1;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            transform.Translate(0, -speed * Time.deltaTime, 0);
            anim.SetInteger("dir", 1);
            anim.speed = 1;
        }
        else if (Input.GetKey(KeyCode.A))
        {
            transform.Translate(-speed * Time.deltaTime, 0, 0);
            anim.SetInteger("dir", 2);
            anim.speed = 1;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            transform.Translate(speed * Time.deltaTime, 0, 0);
            anim.SetInteger("dir", 3);
            anim.speed = 1 ;
        }
        else
            anim.speed = 0;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
      if (col.gameObject.tag == "EnemyBullet")
        {
            if(!iniFrames)
            {
                iniFrames = true;
                currentHealth--;
            }
            col.gameObject.GetComponent<Bullet>().createParticle();
            Destroy(col.gameObject);
        }
      if (col.gameObject.tag == "Potion")
        {
            currentHealth = maxHealth;
            Destroy(col.gameObject);
            if (maxHealth >= 5)
                return;
            maxHealth++;
            currentHealth = maxHealth;
        }
    }

    public void SaveGame()
    {
        PlayerPrefs.SetInt("max", maxHealth);
        PlayerPrefs.SetInt("current", currentHealth);
    }

    void LoadGame()
    {
        maxHealth = PlayerPrefs.GetInt("max");
        currentHealth = PlayerPrefs.GetInt("current");
    }

}
