using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Items : MonoBehaviour
{

    //The id of this item
    public int id = 0;

    //The quantity of this item
    public int quantity = 1;

    private void OnCollisionStay2D(Collision2D other)
    {

        if (other.gameObject.tag.Equals("Player"))
        {
            other.gameObject.GetComponent<PlayerHandler>().AddToInventory(this, quantity);
            Destroy(gameObject);
        }
    }
}
