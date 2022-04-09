using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileMovement : MonoBehaviour
{
    //Speed with wich it moves
    public int speed = 1;

    [SerializeField]
    private int damage;

    public int Damage { get { return damage; } set { damage = value; } }

    // Update is called once per frame
    void Update()
    {

        //Just go forward lolz
        transform.Translate(transform.right * Time.deltaTime * speed);
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.tag.Equals("Player"))
            other.gameObject.GetComponent<Movement>().Knockback(70, -transform.position);
        Destroy(gameObject);
    }
}
