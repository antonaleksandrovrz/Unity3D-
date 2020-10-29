using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public int walkSpeed = 4;
    public int jumpHeight = 400;
    public int damage = 1;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] LayerMask wallLayer;

    [SerializeField] bool facingRight = false;
    public bool isGrounded = false;
    [SerializeField] bool doubleJump = false;
    [SerializeField] GameObject lastJumpedObject;
    public bool canMove = true;
    bool attackingDown;
    GameObject character;
    Animator anim;
    Rigidbody2D rb2D;

    [SerializeField] float jumpRememberTime;
    float jumpRememberTimer;

    void Start()
    {
        character = transform.GetChild(0).gameObject;
        anim = character.GetComponent<Animator>();
        rb2D = GetComponent<Rigidbody2D>();
        
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = Physics2D.OverlapCircle(transform.position, 0.15f, groundLayer);
        anim.SetBool("Grounded", isGrounded);
        anim.SetFloat("Movement", Math.Abs(Input.GetAxis("Horizontal")));

        if (isGrounded)
        {
            doubleJump = true;
            lastJumpedObject = null;
            attackingDown = false;
        }

        else if (attackingDown)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position + new Vector3(0, -0.5f, 0), Vector3.down, 1f);

            if (hit.transform.tag == "Enemy")
            {
                hit.transform.gameObject.GetComponent<Enemy>().GetDamage(damage * 2);
                attackingDown = false;
            }
        }

        jumpRememberTimer -= Time.deltaTime;
        if(Input.GetKeyDown(KeyCode.Space))
        {
            jumpRememberTimer = jumpRememberTime;
        }

        if ((jumpRememberTimer > 0 ) && isGrounded)
        {
            rb2D.velocity = new Vector2(rb2D.velocity.x, jumpHeight);
        }

        else if (Input.GetKeyDown(KeyCode.Space) && !isGrounded && doubleJump)
        {
            rb2D.velocity = new Vector2(rb2D.velocity.x, jumpHeight);
            doubleJump = false;
        }

        else if (Input.GetKeyDown(KeyCode.S) && !isGrounded && !doubleJump)
        {
            attackingDown = true;
            rb2D.velocity = new Vector2(0, -jumpHeight * 2);
            anim.SetTrigger("hitDown");
        }

        if (Input.GetAxis("Horizontal") != 0 && canMove)
        {
            Move();
        }

        if (Input.GetButtonDown("Fire1"))
        {
            Hit();
        }

        if (facingRight)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position + new Vector3(0.7f, 0.5f, 0), Vector3.right, 1f);

            if (hit && hit.collider.tag != "Player")
            {
                if (hit.collider.gameObject != lastJumpedObject)
                {
                    doubleJump = true;
                    lastJumpedObject = hit.collider.gameObject;
                }
            }
        }

        else
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position + new Vector3(-0.7f, 0.5f, 0), Vector3.left, 1f);

            if (hit && hit.collider.tag != "Player")
            {
                if (hit.collider.gameObject != lastJumpedObject)
                {
                    doubleJump = true;
                    lastJumpedObject = hit.collider.gameObject;
                }
            }
        }
    }

    void Move()
    {
        float hor = Input.GetAxis("Horizontal");
        transform.position += transform.right * hor * walkSpeed * Time.deltaTime;
        

        if (hor > 0 && !facingRight)
        {
            character.transform.localRotation = Quaternion.Euler(0, 180, 0);
            facingRight = true;
        }

        else if (hor < 0 && facingRight)
        {
            character.transform.localRotation = Quaternion.Euler(0, 0, 0);
            facingRight = false;
        }

    }

    void Hit()
    {
        if (isGrounded)
        {
            anim.SetTrigger("Hit");
            canMove = false;
        }

        else
        {
            anim.SetTrigger("Hit");
        }

        if (facingRight)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position + new Vector3(0.8f, 0.5f, 0), Vector3.right, 1f);

            if (hit.transform.tag == "Enemy")
            {
                hit.transform.gameObject.GetComponent<Enemy>().GetDamage(damage);
            }

            if (hit.transform.tag == "Crushable")
            {
                hit.transform.gameObject.GetComponent<Vases>().GetDamage();
            }
        }

        else
        {
            Debug.DrawRay(transform.position + new Vector3(-0.8f, 0.5f, 0), Vector3.left, Color.red, 10);
            RaycastHit2D hit = Physics2D.Raycast(transform.position + new Vector3(-0.8f, 0.5f, 0), Vector3.left, 1f);

            if (hit.transform.tag == "Enemy")
            {
                hit.transform.gameObject.GetComponent<Enemy>().GetDamage(damage);
            }

            if (hit.transform.tag == "Crushable")
            {
                hit.transform.gameObject.GetComponent<Vases>().GetDamage();
            }
        }
        
    }


}
