using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    //The much we multiply the gravity when player is falling
    public float fallMultiplier = 1.5f;

    //The multiplier of the jump power
    public float jumpModifier = 2f;

    [HideInInspector]
    public float normalSpd, normalJump;

    //The horizontal collision hit info
    RaycastHit2D hit;

    //Is the player on ground?
    private bool isGrounded = true;

    private AudioSource audioSource;
    public AudioClip audioWalk, audioJump, audioLanding, audioChange, audioSlow;

    [SerializeField]
    private float fallSpeedClampValue = 12;


    public bool IsGrounded { get { return isGrounded; } }

    float mag = 5;

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

        normalSpd = spd;
        normalJump = jumpPow;

        audioSource = GetComponent<AudioSource>();

        AnimationReset();

        UpdateWeaponRenderer();
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

        {
            audioSource.PlayOneShot(audioChange);
        }

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

        //Quão mau querem este código de colisão?
        //Tiago + Vicente: Sim

        RaycastHit2D a = Physics2D.Raycast(new Vector2(transform.position.x - collider_.bounds.extents.x, transform.position.y), Vector2.down, collider_.bounds.extents.y + 0.1f);
        RaycastHit2D b = Physics2D.Raycast(new Vector2(transform.position.x + collider_.bounds.extents.x, transform.position.y), Vector2.down, collider_.bounds.extents.y + 0.1f);
        RaycastHit2D c = Physics2D.Raycast(transform.position, Vector2.down, collider_.bounds.extents.y + 0.1f);

        if (a.collider != null || b.collider != null || c.collider != null)
        {
            isGrounded = true;
        }
        else
        {
            Invoke("notGrounded", 0.1f);
        }

        if (hit.collider == null)
        {
            rigidbody_.transform.Translate(movement.x * Time.deltaTime * spd, 0, 0);

            if (!audioSource.isPlaying && axis != 0 && isGrounded)
            {
                audioSource.PlayOneShot(audioWalk);
            }
        }

    }

    public void UpdateWeaponRenderer()
    {

        WeaponManager.GetCurrentWeapon().GetComponent<SpriteRenderer>().enabled = true;
    }

    public void notGrounded()
    {
        isGrounded = false;
    }

    private void Update()
    {

        if (isGrounded && mag < 2)
        {
            rigidbody_.velocity = Vector2.up * jumpPow;
            mag = 5;
        }
        else
        {
            //Check for jump
            if (Input.GetKeyDown("space") && isGrounded)
            {
                audioSource.PlayOneShot(audioJump);
                rigidbody_.velocity = Vector2.up * jumpPow;
            }

            if (Input.GetKeyDown("space") && !isGrounded)
            {
                RaycastHit2D aux = Physics2D.Raycast(transform.position, Vector2.down, 1f);
                if (aux.collider != null)
                    mag = (transform.position - aux.collider.gameObject.transform.position).magnitude;
            }
        }

        //If we are falling
        if (rigidbody_.velocity.y < 0 && rigidbody_.velocity.y < 2)
        {
            rigidbody_.velocity += Vector2.up * (fallMultiplier - 1) * Physics.gravity.y * Time.deltaTime;
            if (IsGrounded && !audioSource.isPlaying)
            {
                audioSource.PlayOneShot(audioLanding);
            }
        }
        else if (rigidbody_.velocity.y > 0 && !Input.GetKey("space"))
        { //Going up
            rigidbody_.velocity += Vector2.up * (jumpModifier - 1) * Physics.gravity.y * Time.deltaTime;
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

        ClampFallSpeed();
    }

    private void ClampFallSpeed()
    {
        if (rigidbody_.velocity.y > fallSpeedClampValue)
        {
            rigidbody_.velocity = new Vector2(rigidbody_.velocity.x, fallSpeedClampValue);
        }
        else if (rigidbody_.velocity.y < -fallSpeedClampValue)
        {
            rigidbody_.velocity = new Vector2(rigidbody_.velocity.x, -fallSpeedClampValue);
        }
    }

    private void LateUpdate()
    {

        //Update weapon positions
        weaponPos.position = transform.position;

        //If we change direction tweak weapon rotation
        if (WeaponManager.IsCurrentWeaponRotatable())
        {

            if (Input.GetKeyDown(KeyCode.A) && !spriteRenderer.flipX)
            {
                weaponPos.transform.rotation = Quaternion.Inverse(weaponPos.rotation);
            }
            if (Input.GetKeyDown(KeyCode.D) && spriteRenderer.flipX)
            {
                weaponPos.transform.rotation = Quaternion.Inverse(weaponPos.rotation);
            }
        }

        if (axis != 0)
        {
            spriteRenderer.flipX = axis < 0;
            weaponRenderer.flipX = axis < 0;
        }

        if (Input.GetKeyDown(KeyCode.G) && Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene("ExpoColgaia");
            UpdateWeaponRenderer();
        }
    }

    public void Knockback(float strength, Vector3 angle)
    {
        if (GetComponent<PlayerHandler>().hp.Hp > 1)
        {
            rigidbody_.AddForce(-angle * strength * 3);
        }
    }

    public void SpeedDecrease()
    {
        spd = spd / 2;
    }

    private void OnTriggerStay2D(Collider2D other)
    {

        if (other.gameObject.tag == "Slow")
        {
            spd = 1f;
            jumpPow = 4;
            if (!audioSource.isPlaying)
            {
                audioSource.PlayOneShot(audioSlow);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {

        if (other.gameObject.tag == "Slow")
        {

            spd = normalSpd;
            jumpPow = normalJump;
        }
    }
}
