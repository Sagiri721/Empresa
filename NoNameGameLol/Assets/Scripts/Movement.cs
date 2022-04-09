using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    //Component references
    private CapsuleCollider2D collider_;
    private Rigidbody2D rigidbody_;
    private SpriteRenderer spriteRenderer;

    //The movement speed and jump power
    public float spd = 5f, jumpPow = 100;

    //The horizontal collision hit info
    RaycastHit2D hit;

    //Is the player on ground?
    private bool isGrounded = true;

    void Start()
    {
        //Initialize variables
        collider_ = GetComponent<CapsuleCollider2D>();
        rigidbody_ = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void FixedUpdate()
    {
        //Movement code
        //Gets input
        Vector3 movement = new Vector3(Input.GetAxis("Horizontal"), 0, 0);

        //Checks for collision
        hit = Physics2D.BoxCast(transform.position, Vector2.zero, 0, new Vector2(movement.x, 0), collider_.size.x, LayerMask.GetMask("Player", "Collision"));

        isGrounded = Physics2D.Raycast(transform.position, Vector2.down, collider_.bounds.extents.y + 0.1f).collider != null;

        if (hit.collider == null)
            rigidbody_.transform.Translate(movement.x * Time.deltaTime * spd, 0, 0);
    }

    private void Update()
    {
        //Check for jump
        if (Input.GetKeyDown("space") && isGrounded)
        {
            rigidbody_.AddForce(Vector2.up * jumpPow);
        }
    }

    public void Knockback(float strength, Vector3 angle)
    {
        rigidbody_.AddForce(-angle * strength);
    }
}
