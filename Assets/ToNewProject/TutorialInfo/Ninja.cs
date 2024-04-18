using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ninja : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask wallLayer;
    private Rigidbody2D rb;
    private Vector3 averageScale;
    private Animator anim;
    private BoxCollider2D BC;
    private bool isGrounded;
    private float walljump;
    private float horizontalInput; // Moved here for global scope

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        averageScale = transform.localScale;
        anim = GetComponent<Animator>();
        BC = GetComponent<BoxCollider2D>();
    }

    void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");

        rb.velocity = new Vector2(horizontalInput * speed, rb.velocity.y);

        if (horizontalInput < 0)
        {
            transform.localScale = new Vector3(-(averageScale.x), averageScale.y, averageScale.z);
            anim.SetInteger("state", 2);
            transform.Translate(new Vector3(-0.005f, 0, 0));
        }
        else if (horizontalInput > 0)
        {
            transform.localScale = new Vector3((averageScale.x), averageScale.y, averageScale.z);
            anim.SetInteger("state", 2);
            transform.Translate(new Vector3(0.005f, 0, 0));
        }
        else
        {
            anim.SetInteger("state", 1);
        }

        anim.SetBool("IsGrounded", isGrounded); // Update bool parameter bech net2qked li huq on the ground

        if (Input.GetKeyDown(KeyCode.UpArrow) && IsGrounded())
        {
            Jump();
        }
        if (walljump < 0.2f)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow) && IsGrounded())
            {
                Jump();
                rb.velocity = new Vector2(horizontalInput * speed, rb.velocity.y);
                if (wall() && !IsGrounded())
                {
                    rb.gravityScale = 0; // disabling gravity and allowing l plqyer chyeb3ed al hyt maghir mayty7
                    rb.velocity = Vector2.zero; // on the y axis mayaamel hatta state okhra o hatta move
                }
                else
                {
                    rb.gravityScale = 2.5f;
                }
            }
        }
        else
        {
            walljump += Time.deltaTime;
        }
    }

    private void Jump()
    {
        Debug.Log("Jumping");
        rb.velocity = new Vector2(rb.velocity.x, speed);
        isGrounded = false;
        transform.Translate(new Vector3(0, 0.5f, 0));
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "ground")
        {
            Debug.Log("IsGrounded");
            isGrounded = true;
        }
    }

    private bool IsGrounded()
    {
        RaycastHit2D hit = Physics2D.BoxCast(
            BC.bounds.center,
            BC.bounds.size,
            0f,
            Vector2.down,
            0.1f,
            groundLayer
        );

        return hit.collider != null;
    }

    private bool wall()
    {
        Vector2 direction = Vector2.zero;
        if (horizontalInput < 0)
        {
            direction = Vector2.left;
        }
        else if (horizontalInput > 0)
        {
            direction = Vector2.right;
        }

        Vector2 position = new Vector2(
            transform.position.x + (BC.bounds.size.x * 0.5f * direction.x),
            BC.bounds.center.y
        );

        // Perform the BoxCast
        RaycastHit2D hit = Physics2D.BoxCast(
            position,
            BC.bounds.size,
            0f,
            direction,
            0.1f,
            wallLayer
        );

        // Check if the collider exists
        return hit.collider != null;
    }

    public bool canAttack()
    {
        return horizontalInput == 0 && isGrounded && !wall();
    }
}