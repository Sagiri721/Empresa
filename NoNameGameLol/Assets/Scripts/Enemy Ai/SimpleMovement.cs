using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleMovement : MonoBehaviour
{

    Enemy enemy;

    //The speed with which it moves
    public float speed = 1;

    public float radius = 5;

    int size = 0;

    SpriteRenderer spriteRenderer;
    BoxCollider2D boxCollider;

    LineRenderer lr;

    Animator animator;

    GameObject player;

    Vector3 target;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        boxCollider = GetComponent<BoxCollider2D>();

        enemy = GetComponent<Enemy>();

        player = GameObject.FindWithTag("Player");
        animator = GetComponent<Animator>();

        lr = GetComponent<LineRenderer>();
        lr.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {

        if (!animator.GetBool("isCharging") && !lr.enabled)
        {
            spriteRenderer.flipX = speed < 0;
            transform.Translate(speed * Time.deltaTime, 0, 0);

            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.right * Mathf.Sign(speed), boxCollider.bounds.extents.x + 0.1f);

            if (hit.collider != null)
            {
                if (hit.collider.gameObject.layer == 7)
                    speed = -speed;
                else if (hit.collider.gameObject.tag == "Player")
                    hit.collider.gameObject.GetComponent<Movement>().Knockback(20, new Vector3(23 * -speed * Time.deltaTime, -20 * Time.deltaTime, 0));
            }

            if (enemy.GetDistanceFromObject(player) < radius)
            {

                Invoke("SetTarget", 1.85f / 2);

                //Se há paredes no caminho continua
                RaycastHit2D raycast = Physics2D.Raycast(transform.position, target - transform.position, enemy.GetDistanceFromObject(player));
                size = enemy.GetDistanceFromObject(player);

                if (raycast.collider != null && raycast.collider.gameObject.tag != "Player")
                    return;

                animator.SetBool("isCharging", true);
                Vector3[] pos = { transform.position, target };
                lr.SetPositions(pos);

                spriteRenderer.flipX = (target - transform.position).x < 0;

                Invoke("ResetBool", 1.85f);
            }
        }

    }

    private void ResetBool()
    {
        animator.SetBool("isCharging", false);
        lr.enabled = true;

        RaycastHit2D raycast = Physics2D.Raycast(transform.position, target - transform.position, size);

        if(raycast.collider.gameObject.tag == "Player")
        {
            raycast.collider.gameObject.GetComponent<PlayerHandler>().hp.Hurt(50);
        }

        Invoke("Remove", 0.5f);
    }

    private void Remove()
    {

        lr.enabled = false;
    }

    private void SetTarget()
    {
        target = player.transform.position;
    }


}
