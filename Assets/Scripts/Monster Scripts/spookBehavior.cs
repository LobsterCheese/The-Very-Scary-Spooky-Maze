using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

[RequireComponent(typeof(Animator))]

public class spookBehavior : MonoBehaviour
{
    public float currentSpeed;
    /*
    [SerializeField]
    private GameObject textbox;
    [SerializeField]
    private GameObject jumpscareCanvas;
    */

    //this is pretty much all just for spooks
    private Animator anim;
    [SerializeField]
    private RuntimeAnimatorController deathAnim;
    [SerializeField]
    private AnimationClip deathAnimClip;
    public bool dying;

    private AudioSource source;
    [SerializeField]
    private float minDist;
    [SerializeField]
    private float maxDist;

    [SerializeField]
    private GameObject happyStatic;

    [Header("Base Stats")]
    public float mobSpeed = 2f;
    public float mobSlowedSpeed = 1f;
    public float HP = 3f;
    //public Sprite jumpScare;

    [Header("Toggle this on if the enemy uses HP")]
    public bool useHP;

    [Header("Toggle this on if skeleton")]
    public bool skeleton;

    [Header("Toggle this on if Happy Henry")]
    [SerializeField]
    private bool happy;
    //default should be 0f;
    [SerializeField]
    private float happyFaster;

    private GameObject target;
    //private SpriteRenderer sprender;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        anim = GetComponent<Animator>();
        target = GameObject.Find("Guy");
        source = GetComponent<AudioSource>();
        source.volume = 0;
        //sprender = GetComponent<SpriteRenderer>();
        currentSpeed = mobSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        //only moves mob towards player if textbox, death screen, or jumpscare is not active
        if (!happy && !dying)
        {
            transform.position = Vector3.MoveTowards(transform.position, target.transform.position, currentSpeed * Time.deltaTime * playerUpgrades.instance.dontMove);
        }
        else if (happy)
        {
            transform.position = Vector3.MoveTowards(transform.position, target.transform.position, (currentSpeed + happyFaster) * Time.deltaTime * playerUpgrades.instance.dontMove);
        }

        if (happy)
        {
            //happyFaster += 0.1f * Time.deltaTime;
        }

        if (HP <= 0 && !dying)
        {
            dying = true;
            SFXManager.instance.PlaySoundRandom(SFXManager.instance.spookDie);
            anim.runtimeAnimatorController = deathAnim;
            Destroy(gameObject, deathAnimClip.length);
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
            if (playerUpgrades.instance.dontMove == 0f)
            {
                source.volume = 0;
            }
            //if monster is directly on top of player
            else if (dist < minDist)
            {
                if (happy)
                {
                    Debug.Log("on top");

                    if (!happyStatic.activeInHierarchy)
                    {
                        happyStatic.SetActive(true);
                    }

                    Color temp = Color.white;
                    temp.a = 0.3f;
                    happyStatic.GetComponent<Image>().color = temp;

                }

                source.volume = 0.5f;
            }
            //if monster is further away than the max, dont do anything
            else if (dist > maxDist)
            {
                if (happy)
                {
                    if (happyStatic.activeInHierarchy)
                    {
                        Debug.Log("off");
                        happyStatic.SetActive(false);
                    }
                }
                source.volume = 0;
            }
            //if monster is approaching player
            else
            {
                if (happy)
                {

                    if (!happyStatic.activeInHierarchy)
                    {
                        Debug.Log("approaching");
                        happyStatic.SetActive(true);
                    }

                    Color temp = Color.white;
                    temp.a = 0.3f - ((dist - minDist) / (maxDist - minDist));
                    happyStatic.GetComponent<Image>().color = temp;

                }

                source.volume = 0.5f - ((dist - minDist) / (maxDist - minDist));
            }
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        //when mob gets into the flashlight beam, if they have hp, they will take damage
        if(other.gameObject.tag == "Flashlight")
        {
            //happyFaster = 0.2f;

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

        if (other.gameObject.tag == "Freeze" || other.gameObject.tag == "Safe")
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
