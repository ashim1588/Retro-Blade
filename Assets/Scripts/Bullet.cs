using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public GameObject particleEffect;
    float timer = 2f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        if(timer <= 0)
        {
            createParticle();
            Destroy(gameObject);
        }
    }

    public void createParticle()
    {
        Instantiate(particleEffect, transform.position, transform.rotation);
    }
}
