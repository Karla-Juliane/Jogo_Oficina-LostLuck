using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vector2 = System.Numerics.Vector2;

public class EnemyBlue : MonoBehaviour
{
    public float speed;
    public float walkTime;
    public bool walkRight = true;

    public int health;
    public int damage = 1;

    private float timer;
   
    private Animator anim;
    private Rigidbody2D rig;
    // Start is called before the first frame update
    void Start()
    {
        rig = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        timer += Time.deltaTime;

        if (timer >= walkTime)
        {
            walkRight = !walkRight;
            timer = 0f;
        }

        if (walkRight)
        {
            transform.eulerAngles = new Vector3(0, 0);
            rig.velocity = Vector3.right * speed;
        }
        else
        {
            transform.eulerAngles = new Vector3(0, 180);
            rig.velocity = Vector3.left * speed;
        }
    }

    public void Damage(int dmg)
    {
        health -= dmg;
        anim.SetTrigger("dead");

        if (health <= 0)
        {
            //destroi o inimigo
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<Player>().Damage(damage);
        }
    }
}
