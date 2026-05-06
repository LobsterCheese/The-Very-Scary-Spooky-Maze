using UnityEngine;

[RequireComponent(typeof(Animator))]

public class spookBehavior : MonoBehaviour
{
    public float currentSpeed;
    [SerializeField]
    private GameObject textbox;

    [Header("Baase Stats")]
    public float mobSpeed = 2f;
    public float mobSlowedSpeed = 1f;
    public float HP = 3f;
    //public Sprite jumpScare;

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
        //only moves mob towards player if textbox is not active
        if (!textbox.activeInHierarchy)
        {
            //moves mob towards player
            transform.position = Vector3.MoveTowards(transform.position, target.transform.position, currentSpeed * Time.deltaTime);

            if (HP <= 0)
            {
                Destroy(gameObject);
            }
        }

        //faces enemy towards player depending on position as long as they aren't a skeleton
        if (!skeleton)
        {
            if (target.transform.position.x < transform.position.x)
            {
                transform.localScale = new Vector3(-1, 1, 1);
            }
            else
            {
                transform.localScale = new Vector3(1, 1, 1);
            }
        }
    }

    //special check for skeletons, so they only flip their sprite when you can't see them
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Flashlight"))
        {
            if (skeleton)
            {
                if (target.transform.position.x < transform.position.x)
                {
                    sprender.flipX = false;
                }
                else
                {
                    sprender.flipX = true;
                }
            }
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        //when mob gets into the flashlight beam, if they have hp, they will take damage
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
