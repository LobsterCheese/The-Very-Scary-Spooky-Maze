using UnityEngine;

[RequireComponent(typeof(Animator))]

public class spookBehavior : MonoBehaviour
{
    public float currentSpeed;

    [Header("Baase Stats")]
    public float mobSpeed = 2f;
    public float mobSlowedSpeed = 1f;
    public float HP = 3f;
    public Sprite jumpScare;

    [Header("Toggle this on if the enemy uses HP")]
    public bool useHP;

    [Header("Toggle this on if skeleton")]
    public bool skeleton;

    private GameObject target;
    private SpriteRenderer sprender;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        target = GameObject.Find("Guy");
        sprender = GetComponent<SpriteRenderer>();
        currentSpeed = mobSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, target.transform.position, currentSpeed * Time.deltaTime);

        if (HP <= 0)
        {
            Destroy(gameObject);
        }

        //faces enemy towards player depending on position
        if (target.transform.position.x < transform.position.x)
        {
            sprender.flipX = false;
        }
        else
        {
            sprender.flipX = true;
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if(other.gameObject.tag == "Flashlight")
        {
            currentSpeed = mobSlowedSpeed;
            if (useHP)
            {
                HP -= Time.deltaTime;
            }
        }

        if(other.gameObject.tag == "Freeze")
        {
            currentSpeed = 0;
        }

    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Flashlight" || (other.gameObject.tag == "Freeze"))
        {
            currentSpeed = mobSpeed;
        }
    }

}
