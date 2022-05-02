using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spring : MonoBehaviour
{

    Animator animator;

    private AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (!audioSource.isPlaying)
            {
                audioSource.Play();
            }
            other.GetComponent<Movement>().Knockback(260, Vector3.down);
            animator.SetBool("isActive", true);
            Invoke("isnotactive", 0.05f);
        }
    }

    public void isnotactive()
    {
        animator.SetBool("isActive", false);
    }
}
