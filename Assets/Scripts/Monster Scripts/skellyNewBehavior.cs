using UnityEngine;
using UnityEngine.AI;
using static Unity.VisualScripting.Member;

public class skellyNewBehavior : MonoBehaviour
{
    //reference objects
    [SerializeField]
    Transform target;
    [SerializeField]
    private AudioClip[] skellySounds;

    //audio stuff
    [SerializeField]
    private float minDist;
    [SerializeField]
    private float maxDist;

    //stuff attached to skelly
    NavMeshAgent agent;
    SpriteRenderer sprender;
    private AudioSource source;
    //initial speed of skelly
    private float startSpeed;
    //for flashlight control
    public float moveMult;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        sprender = GetComponent<SpriteRenderer>();

        source = GetComponent<AudioSource>();

        startSpeed = agent.speed;

        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }

    // Update is called once per frame
    void Update()
    {
        //moves towards player if no textbox, death screen, etc
        agent.speed = startSpeed * moveMult * playerUpgrades.instance.dontMove;

        agent.SetDestination(target.position);

        //gets distance from monster to player
        float dist = Vector3.Distance(transform.position, target.transform.position);

        //if the death screen is active, sound will be 0
        if (playerUpgrades.instance.dontMove == 0f)
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

    //special check for skeletons, so they only flip their sprite when you can't see them
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Flashlight") || collision.gameObject.CompareTag("Marker"))
        {
            PlaySoundRandom(skellySounds);
        }


        if (collision.gameObject.CompareTag("Flashlight") || collision.gameObject.CompareTag("Marker") || collision.gameObject.tag == "Freeze")
        {
            moveMult = 0f;

            if (target.transform.position.x < transform.position.x && moveMult != 0)
            {
                sprender.flipX = false;
            }
            else
            {
                sprender.flipX = true;
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Flashlight") || collision.gameObject.CompareTag("Marker") || collision.gameObject.tag == "Freeze" || collision.gameObject.tag == "Safe")
        {
            //cantMove = true;
            moveMult = 0f;
        }
    }

    //can move when not under freeze
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Flashlight" || (other.gameObject.tag == "Freeze") || (other.gameObject.tag == "Marker"))
        {
            moveMult = 1f;
        }
    }

    public void PlaySoundRandom(AudioClip[] clips)
    {
        AudioClip clip = clips[Random.Range(0, clips.Length)];
        source.pitch = Random.Range(0.80f, 1.10f);
        source.PlayOneShot(clip);
    }

}
