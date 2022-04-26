using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spring : MonoBehaviour
{

    Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {

            other.GetComponent<Movement>().Knockback(200, Vector3.down);
            animator.SetBool("isActive", true);
            Invoke("isnotactive", 0.05f);
        }
    }

    public void isnotactive()
    {
        animator.SetBool("isActive", false);
    }
}
