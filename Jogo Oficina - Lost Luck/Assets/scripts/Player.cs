using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int health = 3;
    public float speed;
    public float jumpForce;

    public GameObject effect;
    public Transform firePoint;
    
    private bool isJumping;
    private bool doubleJump;
    private bool isAtk;
        
    private Rigidbody2D rig;
    private Animator anim;

    private float movement;

    private Vector3 respawnPoint;

    public GameObject fallDetector;
    // Start is called before the first frame update
    void Start()
    {
        rig = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        respawnPoint = transform.position;
        
        GameController.instance.UpdateLives(health);
    }

    // Update is called once per frame
    void Update()
    {
        Jump();
        Atack();
    }

    private void FixedUpdate()
    {
        Move();
    }

    void Move()
    { 
        movement = Input.GetAxis("Horizontal");

        rig.velocity = new Vector2(movement * speed, rig.velocity.y);

        if (movement > 0)
        {
            if (!isJumping)
            {
                anim.SetInteger("transition", 1);
            }
            transform.eulerAngles = new Vector3(0, 0, 0);
        }

        if (movement < 0)
        {
            if (!isJumping)
            {
                anim.SetInteger("transition", 1);
            }
            transform.eulerAngles = new Vector3(0, 180, 0);
        }

        if (movement == 0 && !isJumping && !isAtk)
        {
            anim.SetInteger("transition", 0);
        }

        fallDetector.transform.position = new Vector2(transform.position.x, fallDetector.transform.position.y);
    }

    void Jump()
    {
        if(Input.GetButtonDown("Jump"))
        {
            if(!isJumping)
            {
                anim.SetInteger("transition", 2);
                rig.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
                doubleJump = true;
                isJumping = true;
            }
            else
            {
                if(doubleJump)
                {
                    anim.SetInteger("transition", 2);
                    rig.AddForce(new Vector2(0, jumpForce * 2), ForceMode2D.Impulse);
                    doubleJump = false;
                }
            }
        }
    }

    void Atack()
    {
        StartCoroutine("Atk");
    }

    IEnumerator Atk()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            isAtk = true;
            anim.SetInteger("transition", 3);
            GameObject Effect = Instantiate(effect, firePoint.position, firePoint.rotation);

            if (transform.rotation.y == 0)
            {
                Effect.GetComponent<Effect>().isRight = true;
            }

            if (transform.rotation.y == 180)
            {
                Effect.GetComponent<Effect>().isRight = false;
            }
            yield return new WaitForSeconds(0.2f);
            isAtk = false;
            anim.SetInteger("transition", 0);
        }
    }

    public void Damage(int dmg)
    {
        health -= dmg;
        GameController.instance.UpdateLives(health);
        anim.SetTrigger("hurt");
        
        if (transform.rotation.y == 0)
        {
            transform.position += new Vector3(-0.5f, 0, 0);
        }

        if (transform.rotation.y == 180)
        {
            transform.position += new Vector3(0.5f, 0, 0);
        }

        if (health <= 0)
        {
            //chama game over
            GameController.instance.GameOver();
        }
    }

    public void IncreaseLife(int value)
    {
        health += value;
        GameController.instance.UpdateLives(health);
    }

    private void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.layer == 6)
        {
            isJumping = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "FallDetector")
        {
            transform.position = respawnPoint;
        }
        else if (collision.tag == "CheckPoint")
        {
            respawnPoint = transform.position;
        }
    }
}
