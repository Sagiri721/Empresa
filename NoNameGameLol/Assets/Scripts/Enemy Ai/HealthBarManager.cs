using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBarManager : MonoBehaviour
{
    HealthSystem hp;

    public Transform hpSize;

    // Start is called before the first frame update
    void Start()
    {
        hp = GetComponent<Enemy>().GetHealthSystem();
    }

    // Update is called once per frame
    void Update()
    {
        hpSize.localScale = new Vector3(hp.FormatHealth() / 2, 0.03f, 0);
    }
}
