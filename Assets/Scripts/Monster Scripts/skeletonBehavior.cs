using UnityEngine;

public class skeletonBehavior : MonoBehaviour
{

    public Sprite[] skellyStills;

    private Rigidbody2D rb;

    private bool freeze;

    private SpriteRenderer sprender;
    private int randPose;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        sprender = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        //skellys will cycle through random poses until they are seen
        if (!freeze)
        {
            randPose = Random.Range(0, skellyStills.Length);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //sets skelly to a random pose
        if (!freeze)
        {
            sprender.sprite = skellyStills[randPose];
        }

        //plays skeleton sound if player spots it
        if (collision.gameObject.CompareTag("Flashlight") || collision.gameObject.CompareTag("Marker"))
        {
            SFXManager.instance.PlaySoundRandom(SFXManager.instance.skelly);
        }
    }

    //freezes while it's in flashlight
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Flashlight") || collision.gameObject.CompareTag("Marker") || collision.gameObject.CompareTag("animFreeze"))
        {
            freeze = true;
        }
    }

    //moves while it's not in flashlight
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Flashlight") || collision.gameObject.CompareTag("Marker") || collision.gameObject.CompareTag("animFreeze"))
        {
            freeze = false;
        }
    }

}
