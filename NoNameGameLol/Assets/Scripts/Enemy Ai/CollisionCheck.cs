using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionCheck : MonoBehaviour
{
    SimpleMovement sm;

    // Start is called before the first frame update
    void Start()
    {
        sm = GetComponentInParent<SimpleMovement>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerExit2D(Collider2D other)
    {
        sm.speed = -sm.speed;
    }
}
