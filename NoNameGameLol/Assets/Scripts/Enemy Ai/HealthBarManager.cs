using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBarManager : MonoBehaviour
{
    HealthSystem hp;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.localScale = new Vector3(1, 0.03f, 0);
    }
}
