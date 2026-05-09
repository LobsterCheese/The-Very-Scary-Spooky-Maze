using UnityEngine;
using UnityEngine.Audio;

[RequireComponent(typeof(Animator))]

public class spookBehavior : MonoBehaviour
{
    public float currentSpeed;
    [SerializeField]
    private GameObject textbox;
    [SerializeField]
    private GameObject jumpscareCanvas;


    private AudioSource source;
    [SerializeField]
    private float minDist;
    [SerializeField]
    private float maxDist;

    [Header("Base Stats")]
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
        source = GetComponent<AudioSource>();
        source.volume = 0;
        sprender = GetComponent<SpriteRenderer>();
        currentSpeed = mobSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        //only moves mob towards player if textbox, death screen, or jumpscare is not active
        transform.position = Vector3.MoveTowards(transform.position, target.transform.position, currentSpeed * Time.deltaTime * playerUpgrades.instance.dontMove);

        if (HP <= 0)
        {
            SFXManager.instance.PlaySoundRandom(SFXManager.instance.spookDie);
            Destroy(gameObject);
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

        //sfx handling
        if (currentSpeed != 0 && !skeleton) {

            //gets distance from monster to player
            float dist = Vector3.Distance(transform.position, target.transform.position);

            //if the death screen is active, sound will be 0
            if (jumpscareCanvas.activeInHierarchy || textbox.activeInHierarchy)
            {
                source.volume = 0;
            }
            else if (dist < minDist)
            {
                source.volume = 0.5f;
            }
            else if (dist > maxDist)
            {
                source.volume = 0;
            }
            else
            {
                source.volume = 0.5f - ((dist - minDist) / (maxDist - minDist));
            }
        }
    }

    //special check for skeletons, so they only flip their sprite when you can't see them
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Flashlight") || collision.gameObject.CompareTag("Marker"))
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

        //when mob gets into the marker light, they dont get the full slow and only take half the amount of damage
        if (other.gameObject.tag == "Marker")
        {
            currentSpeed = mobSlowedSpeed * 1.5f;
            if (useHP)
            {
                HP -= 0.5f * Time.deltaTime;
            }
        }

        if (other.gameObject.tag == "Freeze")
        {
            currentSpeed = 0;
        }

    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Flashlight" || (other.gameObject.tag == "Freeze") || (other.gameObject.tag == "Marker"))
        {
            currentSpeed = mobSpeed;
        }
    }

}
