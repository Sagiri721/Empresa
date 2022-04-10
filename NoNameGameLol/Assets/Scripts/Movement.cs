using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    //Component references
    private CapsuleCollider2D collider_;
    private Rigidbody2D rigidbody_;
    private SpriteRenderer spriteRenderer, weaponRenderer;

    //Arm tracking
    private Animator animator, weaponAnimator;
    private Transform weaponPos;

    float axis = 0;

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
        animator = GetComponent<Animator>();

        var weapon = WeaponManager.GetCurrentWeapon();

        weaponRenderer = weapon.GetComponent<SpriteRenderer>();
        weaponAnimator = weapon.GetComponent<Animator>();
        weaponPos = weapon.GetComponent<Transform>();

        AnimationReset();
    }

    private void AnimationReset()
    {

        animator.SetBool("isIdle", true);
        animator.SetBool("isOnAir", false);

        weaponAnimator.SetBool("isIdle", animator.GetBool("isIdle"));
        weaponAnimator.SetBool("isOnAir", animator.GetBool("isOnAir"));
    }

    public void ChangeWeapon(GameObject weapon)
    {

        weaponRenderer = weapon.GetComponent<SpriteRenderer>();
        weaponAnimator = weapon.GetComponent<Animator>();
        weaponPos = weapon.GetComponent<Transform>();

        weaponRenderer.flipX = spriteRenderer.flipX;

        AnimationReset();
    }

    private void FixedUpdate()
    {
        axis = Input.GetAxis("Horizontal");
        //Movement code
        //Gets input
        Vector3 movement = new Vector3(axis, 0, 0);

        //Checks for collision
        hit = Physics2D.BoxCast(transform.position, Vector2.zero, 0, new Vector2(movement.x, 0), collider_.size.x, LayerMask.GetMask("Player", "Collision"));

        RaycastHit2D a = Physics2D.Raycast(new Vector2(transform.position.x - collider_.bounds.extents.x, transform.position.y), Vector2.down, collider_.bounds.extents.y + 0.1f);
        RaycastHit2D b = Physics2D.Raycast(new Vector2(transform.position.x + collider_.bounds.extents.x, transform.position.y), Vector2.down, collider_.bounds.extents.y + 0.1f);

        isGrounded = a.collider != null || b.collider != null;

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

        /*
        Now for the animations
        Animation param names:
            isIdle : bool
            isOnAir : bool
        */

        animator.SetBool("isIdle", axis == 0);
        animator.SetBool("isOnAir", !isGrounded);

        weaponAnimator.SetBool("isIdle", animator.GetBool("isIdle"));
        weaponAnimator.SetBool("isOnAir", animator.GetBool("isOnAir"));
    }

    private void LateUpdate()
    {

        //Update weapon positions
        weaponPos.position = transform.position;

        //If we change direction tweak weapon rotation
        if (WeaponManager.IsCurrentWeaponRotatable())
        {
            if (Input.GetKeyDown(KeyCode.A) && !weaponRenderer.flipX)
                weaponPos.rotation = Quaternion.Euler(0, 0, 360 - weaponPos.eulerAngles.z);
            else if (Input.GetKeyDown(KeyCode.D) && weaponRenderer.flipX)
                weaponPos.rotation = Quaternion.Euler(0, 0, 360 - weaponPos.eulerAngles.z);
        }

        if (axis != 0)
        {
            spriteRenderer.flipX = axis < 0;
            weaponRenderer.flipX = axis < 0;
        }
    }

    public void Knockback(float strength, Vector3 angle)
    {
        rigidbody_.AddForce(-angle * strength * 3);
    }
}
