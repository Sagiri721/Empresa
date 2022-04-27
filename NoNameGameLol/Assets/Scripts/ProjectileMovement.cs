using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileMovement : MonoBehaviour
{
    //Speed with wich it moves
    //Knockback strength
    public int speed = 1, knockbackPow = 30;

    [SerializeField]
    private int damage;

    [SerializeField]
    private bool hasTrail;

    LineRenderer lineRenderer;
    private float trailSize = 0;

    //Unknown
    //Lista de sprites
    public Sprite[] sprites = null;
    //Sprite atual
    public static int currentSprite = 0;

    //The direction in with it moves
    public Vector3 dir = Vector3.zero;

    [SerializeField]
    public GameObject Exprosion;
    private GameObject O;

    public int Damage { get { return damage; } set { damage = value; } }

    private void Start()
    {
        if (hasTrail)
            lineRenderer = GetComponent<LineRenderer>();

        //Ver se temos de trocar sprites
        if (sprites != null)
        {
            if (currentSprite < sprites.Length)
            {
                GetComponent<SpriteRenderer>().sprite = sprites[currentSprite];
                currentSprite++;
            }
            else
            {
                currentSprite = 0;
            }

        }
    }

    // Update is called once per frame
    void Update()
    {

        //Just go forward lolz
        if (GetComponent<SpriteRenderer>().enabled)
            transform.Translate(dir * Time.deltaTime * speed, Space.World);

        if (hasTrail)
        {
            //Update the trail effect
            Vector3[] pos = { transform.position, transform.position - dir.normalized * trailSize };
            lineRenderer.SetPositions(pos);
        }

        if (trailSize < 1f)
            trailSize += 0.005f;
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        if (GetComponent<SpriteRenderer>().enabled)
        {
            if (other.gameObject.tag.Equals("Player"))
                other.gameObject.GetComponent<Movement>().Knockback(knockbackPow, -transform.position);
            GetComponent<SpriteRenderer>().enabled = false;
            GetComponent<LineRenderer>().enabled = false;
            O = Instantiate(Exprosion, transform.position, transform.rotation);
            Invoke("ripzaoExprosion", 0.5f);
        }
    }

    public void ripzaoExprosion()
    {
        Destroy(O.gameObject);
        Destroy(gameObject);
    }
}
